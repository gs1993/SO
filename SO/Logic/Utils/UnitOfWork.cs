using System;
using System.Linq;
using System.Threading.Tasks;
using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic.Utils
{
    public class UnitOfWork : IDisposable
    {
        private readonly StackOverflow2010Context _context;
        private bool _isAlive = true;
        private bool _isCommitted;

        public UnitOfWork(StackOverflow2010Context context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (!_isAlive)
                return;

            _isAlive = false;

            try
            {
                if (_isCommitted)
                {
                    _context.SaveChanges();
                }
            }
            finally
            {
                _context.Dispose();
            }
        }

        public void Commit()
        {
            if (!_isAlive)
                return;

            _isCommitted = true;
        }

        internal T Get<T>(long id) where T : Entity
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        internal Task<T> GetAsync<T>(long id) where T : Entity
        {
            return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        internal void SaveOrUpdate<T>(T entity)
        {
            //_context.SaveChanges(entity);
        }

        internal void Delete<T>(T entity)
        {
            //_context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Query<T>()
        {
            return null; //_session.Query<T>();
        }
    }
}
