using System.Threading.Tasks;
using Logic.Models;
using Logic.Utils;

namespace Logic.Repositories
{
    public abstract class Repository<T> where T : Entity
    {
        protected readonly UnitOfWork _unitOfWork;

        protected Repository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public T GetById(int id)
        {
            return _unitOfWork.Get<T>(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _unitOfWork.GetAsync<T>(id);
        }

        public void Add(T entity)
        {
            _unitOfWork.SaveOrUpdate(entity);
        }
    }
}
