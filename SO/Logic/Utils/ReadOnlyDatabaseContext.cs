using Microsoft.EntityFrameworkCore;

namespace Logic.Utils
{
    public interface IReadOnlyDatabaseContext
    {

    }

    public class ReadOnlyDatabaseContext : IReadOnlyDatabaseContext
    {
        private readonly DatabaseContext _context;

        public ReadOnlyDatabaseContext(DatabaseContext context, QueryConnectionString queryConnectionString)
        {
            _context = context;
            _context.Database.SetConnectionString(queryConnectionString.ConnectionString);
        }
    }
}
