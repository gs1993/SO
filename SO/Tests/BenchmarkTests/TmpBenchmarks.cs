using BenchmarkDotNet.Attributes;
using Logic.BoundedContexts.Posts.Queries;
using Logic.Utils.Db;
using Microsoft.Extensions.DependencyInjection;

namespace BenchmarkTests
{
    [MemoryDiagnoser]
    public class TmpBenchmarks
    {
        //private ReadOnlyDatabaseContext _context;

        //[GlobalSetup]
        //public void GlobalSetup()
        //{
        //    var services = new ServiceCollection();

        //    services.AddDbContexts("",
        //        "");

        //    var serviceProvider = services.BuildServiceProvider();
        //    _context = serviceProvider.GetService<ReadOnlyDatabaseContext>() ?? throw new NullReferenceException();
        //}

        //[Benchmark]
        //public async Task GetPostQueryHandlerAsSplitQueryBenchmark()
        //{
        //    var handler = new GetPostQueryHandler(_context);

        //    await handler.Handle(new GetPostQuery(8051161), CancellationToken.None);
        //}

        //[Benchmark]
        //public async Task GetPostQueryHandlerAsSingleQueryBenchmark()
        //{
        //    var handler = new GetPostQueryHandler(_context);

        //    await handler.Handle2(new GetPostQuery(8051161), CancellationToken.None);
        //}
    }
}
