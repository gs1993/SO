namespace Logic.Utils.Db.Options
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; init; }
        public int CommandTimeoutInSeconds { get; init; }
        public bool EnableDatailedErrors { get; init; }
        public bool EnableSensitiveDataLogging { get; init; }
    }
}
