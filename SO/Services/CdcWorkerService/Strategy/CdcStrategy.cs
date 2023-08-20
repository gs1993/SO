using CdcWorkerService.Db.Models;
using Nest;

namespace CdcWorkerService.Strategy
{
    internal abstract class CdcStrategy
    {
        public abstract Task ExecuteStrategy(PostCT cdcPost, ElasticClient elasticClient, CancellationToken ct);
    }
}
