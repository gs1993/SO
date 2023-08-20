using CdcWorkerService.Db.Models;
using CdcWorkerService.EsClient;
using Nest;

namespace CdcWorkerService.Strategy
{
    internal class InsertCdcStrategy : CdcStrategy
    {
        public override async Task ExecuteStrategy(PostCT cdcPost, ElasticClient elasticClient, CancellationToken ct)
        {
            var dto = new PostIndex(cdcPost);

            var response = await elasticClient.IndexDocumentAsync(dto, ct);
            if (!response.IsValid)
                throw new Exception(response.ServerError.ToString());
        }
    }
}
