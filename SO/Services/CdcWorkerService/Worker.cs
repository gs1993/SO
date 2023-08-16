using CdcWorkerService.Db;
using CdcWorkerService.Db.Models;
using CdcWorkerService.Strategy;
using CdcWorkerService.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Data;

namespace CdcWorkerService;

internal class Worker : BackgroundService
{
    private static readonly string ChangeTrackingTableName = "dbo_Posts_CT";

    private readonly ILogger<Worker> _logger;
    private readonly ElasticClient _elasticClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Dictionary<OperationEnum, CdcStrategy> _strategyContext;
    private readonly WorkerSettings _workerSettings;

    public Worker(ILogger<Worker> logger, ElasticClient elasticClient, IServiceScopeFactory serviceScopeFactory, 
        Dictionary<OperationEnum, CdcStrategy> strategyContext, WorkerSettings workerSettings)
    {
        _logger = logger;
        _elasticClient = elasticClient;
        _serviceScopeFactory = serviceScopeFactory;
        _strategyContext = strategyContext;
        _workerSettings = workerSettings;
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

            await Task.Delay(_workerSettings.DelayInMs, stoppingToken);
        }
    }

    private async Task ProcessChanges(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var lastTracking = await GetLastTracking(dbContext, stoppingToken);
        var minLsn = await GetMinLsn(dbContext, lastTracking);
        var maxLsn = GetMaxLsn(dbContext);

        if (maxLsn != null && !minLsn.SequenceEqual(maxLsn))
        {
            var changes = await GetNetChangesAsync(dbContext, minLsn, maxLsn);
            await ExecuteStrategies(changes, stoppingToken);

            await UpdateLastStartLsn(dbContext, lastTracking, maxLsn, stoppingToken);
        }
    }

    private static async Task<CdcTracking?> GetLastTracking(DatabaseContext dbContext, CancellationToken stoppingToken)
    {
        return await dbContext.CdcTrackings
            .SingleOrDefaultAsync(x => x.TableName == ChangeTrackingTableName, cancellationToken: stoppingToken);
    }

    private static async Task<byte[]> GetMinLsn(DatabaseContext dbContext, CdcTracking? lastTracking)
    {
        return lastTracking == null
            ? await dbContext.PostsCT.Select(c => DatabaseContext.GetMinLsn("dbo_Posts")).FirstAsync()
            : lastTracking.LastStartLsn;
    }

    private static byte[]? GetMaxLsn(DatabaseContext dbContext)
    {
        return dbContext.PostsCT
            .Select(c => DatabaseContext.GetMaxLsn())
            .SingleOrDefault();
    }

    private static async Task<List<PostCT>> GetNetChangesAsync(DatabaseContext dbContext, byte[] fromLsn, byte[] toLsn)
    {
        using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = $"SELECT * FROM cdc.fn_cdc_get_net_changes_dbo_Posts(@from_lsn, @to_lsn, @row_filter_option);";
        command.CommandType = CommandType.Text;

        command.Parameters.Add(new SqlParameter("@from_lsn", fromLsn));
        command.Parameters.Add(new SqlParameter("@to_lsn", toLsn));
        command.Parameters.Add(new SqlParameter("@row_filter_option", "all"));

        await dbContext.Database.OpenConnectionAsync();

        using var result = await command.ExecuteReaderAsync();
        var posts = new List<PostCT>();

        while (await result.ReadAsync())
        {
            var post = new PostCT
            {
                Operation = (OperationEnum)result.GetInt32(result.GetOrdinal("__$operation")),
                Id = result.GetInt32(result.GetOrdinal("Id")),
                Title = result.IsDBNull(result.GetOrdinal("Title")) ? null : result.GetString(result.GetOrdinal("Title")),
                Body = result.IsDBNull(result.GetOrdinal("Body")) ? null : result.GetString(result.GetOrdinal("Body")),
                Score = result.IsDBNull(result.GetOrdinal("Score")) ? null : (int?)result.GetInt32(result.GetOrdinal("Score")),
                Tags = result.IsDBNull(result.GetOrdinal("Tags")) ? null : result.GetString(result.GetOrdinal("Tags")),
                AnswerCount = result.IsDBNull(result.GetOrdinal("AnswerCount")) ? null : (int?)result.GetInt32(result.GetOrdinal("AnswerCount")),
                ClosedDate = result.IsDBNull(result.GetOrdinal("ClosedDate")) ? null : (DateTime?)result.GetDateTime(result.GetOrdinal("ClosedDate")),
                CommentCount = result.IsDBNull(result.GetOrdinal("CommentCount")) ? null : (int?)result.GetInt32(result.GetOrdinal("CommentCount")),
                CreateDate = result.IsDBNull(result.GetOrdinal("CreateDate")) ? null : (DateTime?)result.GetDateTime(result.GetOrdinal("CreateDate")),
                CommunityOwnedDate = result.IsDBNull(result.GetOrdinal("CommunityOwnedDate")) ? null : (DateTime?)result.GetDateTime(result.GetOrdinal("CommunityOwnedDate")),
                FavoriteCount = result.IsDBNull(result.GetOrdinal("FavoriteCount")) ? null : (int?)result.GetInt32(result.GetOrdinal("FavoriteCount")),
                LastActivityDate = result.IsDBNull(result.GetOrdinal("LastActivityDate")) ? null : (DateTime?)result.GetDateTime(result.GetOrdinal("LastActivityDate")),
                LastEditorDisplayName = result.IsDBNull(result.GetOrdinal("LastEditorDisplayName")) ? null : result.GetString(result.GetOrdinal("LastEditorDisplayName")),
                ViewCount = result.IsDBNull(result.GetOrdinal("ViewCount")) ? null : (int?)result.GetInt32(result.GetOrdinal("ViewCount")),
                IsDeleted = result.IsDBNull(result.GetOrdinal("IsDeleted")) ? null : (bool?)result.GetBoolean(result.GetOrdinal("IsDeleted"))
            };

            posts.Add(post);
        }

        await dbContext.Database.CloseConnectionAsync();

        return posts;
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
