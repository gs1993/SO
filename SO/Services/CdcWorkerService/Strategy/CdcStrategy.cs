using CdcWorkerService.Db.Models;
using CdcWorkerService.EsClient;
using Nest;

namespace CdcWorkerService.Strategy
{
    internal abstract class CdcStrategy
    {
        public abstract Task ExecuteStrategy(PostCT cdcPost, ElasticClient elasticClient, CancellationToken ct);
    }

    internal class DeleteCdcStrategy : CdcStrategy
    {
        public override async Task ExecuteStrategy(PostCT cdcPost, ElasticClient elasticClient, CancellationToken ct)
        {
            var response = await elasticClient.DeleteAsync<PostIndex>(cdcPost.Id, ct: ct);
            if (!response.IsValid)
                throw new Exception(response.ServerError.ToString());
        }
    }

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

    internal class ValueAfterUpdateCdcStrategy : CdcStrategy
    {
        public override async Task ExecuteStrategy(PostCT cdcPost, ElasticClient elasticClient, CancellationToken ct)
        {
            var dto = new PostIndex(cdcPost);

            var response = await elasticClient.UpdateAsync<PostIndex>(dto.Id, u => u.Doc(dto), ct: ct);
            if (!response.IsValid)
                throw new Exception(response.ServerError.ToString());
        }
    }
}
