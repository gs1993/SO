using CdcWorkerService.Db.Models;
using CdcWorkerService.EsClient;
using Nest;

namespace CdcWorkerService.Strategy
{
    internal class DeleteCdcStrategy : CdcStrategy
    {
        public override async Task ExecuteStrategy(PostCT cdcPost, ElasticClient elasticClient, CancellationToken ct)
        {
            var response = await elasticClient.DeleteAsync<PostIndex>(cdcPost.Id, ct: ct);
            if (!response.IsValid)
                throw new Exception(response.ServerError.ToString());
        }
    }
}
