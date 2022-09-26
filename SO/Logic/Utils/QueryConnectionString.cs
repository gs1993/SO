namespace Logic.Utils
{
    public class QueryConnectionString
    {
        public string ConnectionString { get; init; }

        public QueryConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}