using System.Collections.Generic;
using System.Linq;
using Logic.Models;
using Logic.Utils;

namespace Logic.Repositories
{
    public class PostsRepository : Repository<Posts>
    {
        public PostsRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IReadOnlyList<Posts> GetList()
        {
            return _unitOfWork.Query<Posts>().ToList();
        }
    }
}
