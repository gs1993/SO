namespace CdcWorkerService.Db.Models
{
    internal class CdcTracking
    {
        public string TableName { get; init; }
        public byte[] LastStartLsn { get; set; }
    }
}
