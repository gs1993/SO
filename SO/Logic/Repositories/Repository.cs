using Logic.Models;

namespace Logic.Repositories
{
    public abstract class Repository<T> where T : Entity
    {
        //protected readonly UnitOfWork unitOfWork;

        //protected Repository(UnitOfWork unitOfWork)
        //{
        //    this.unitOfWork = unitOfWork;
        //}

        //public T GetById(int id)
        //{
        //    return unitOfWork.Get<T>(id);
        //}

        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await unitOfWork.GetAsync<T>(id);
        //}

        //public void Add(T entity)
        //{
        //    unitOfWork.SaveOrUpdate(entity);
        //}
    }
}
