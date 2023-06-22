using CdcWorkerService.Db;
using CdcWorkerService.Db.Models;
using CdcWorkerService.Strategy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace CdcWorkerService;

internal class Worker : BackgroundService
{
    private static readonly string ChangeTrackingTableName = "dbo_Posts_CT";

    private readonly ILogger<Worker> _logger;
    private readonly ElasticClient _elasticClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Dictionary<OperationEnum, CdcStrategy> _strategyContext;

    public Worker(ILogger<Worker> logger, ElasticClient elasticClient,
        IServiceScopeFactory serviceScopeFactory, Dictionary<OperationEnum, CdcStrategy> strategyContext)
    {
        _logger = logger;
        _elasticClient = elasticClient;
        _serviceScopeFactory = serviceScopeFactory;
        _strategyContext = strategyContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessChanges(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Process exception", ex);
                throw;
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessChanges(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var lastTracking = await GetLastTracking(dbContext, stoppingToken);
        var minLsn = GetMinLsn(dbContext, lastTracking);
        var maxLsn = GetMaxLsn(dbContext);

        if (!minLsn.SequenceEqual(maxLsn))
        {
            var changes = await GetNetChangesAsync(dbContext, minLsn, maxLsn);
            await ExecuteStrategies(changes, stoppingToken);

            await UpdateLastStartLsn(dbContext, lastTracking, maxLsn, stoppingToken);
        }
    }

    private static async Task<CdcTracking?> GetLastTracking(DatabaseContext dbContext, CancellationToken stoppingToken)
    {
        return await dbContext.CdcTrackings
            .FirstOrDefaultAsync(x => x.TableName == ChangeTrackingTableName, cancellationToken: stoppingToken);
    }

    private static byte[] GetMinLsn(DatabaseContext dbContext, CdcTracking? lastTracking)
    {
        return lastTracking == null
            ? dbContext.PostsCT.Select(c => DatabaseContext.GetMinLsn("dbo_Posts")).First()
            : lastTracking.LastStartLsn;
    }

    private static byte[] GetMaxLsn(DatabaseContext dbContext)
    {
        return dbContext.PostsCT
            .Select(c => DatabaseContext.GetMaxLsn())
            .First();
    }

    private async Task<List<PostCT>> GetNetChangesAsync(DatabaseContext dbContext, byte[] fromLsn, byte[] toLsn)
    {
        var changes = await dbContext.Set<PostCT>()
            .FromSqlRaw("SELECT * FROM cdc.fn_cdc_get_net_changes_dbo_Posts(@fromLsn, @toLsn, 'all')",
                new SqlParameter("@fromLsn", fromLsn),
                new SqlParameter("@toLsn", toLsn))
            .ToListAsync();

        return changes;
    }

    private async Task ExecuteStrategies(List<PostCT> changes, CancellationToken stoppingToken)
    {
        foreach (var change in changes)
        {
            if (_strategyContext.TryGetValue(change.Operation, out var strategy))
                await strategy.ExecuteStrategy(change, _elasticClient, stoppingToken);
        }
    }

    private static async Task UpdateLastStartLsn(DatabaseContext dbContext, CdcTracking? lastTracking, byte[] maxLsn, CancellationToken stoppingToken)
    {
        if (lastTracking == null)
        {
            dbContext.CdcTrackings.Add(new CdcTracking
            {
                TableName = ChangeTrackingTableName,
                LastStartLsn = maxLsn
            });
        }
        else
        {
            lastTracking.LastStartLsn = maxLsn;
            dbContext.CdcTrackings.Update(lastTracking);
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}
