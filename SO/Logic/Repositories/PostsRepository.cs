using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Models;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repositories
{
    public class PostsRepository : Repository<Posts>
    {
        public PostsRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IReadOnlyList<Posts>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Query<Posts>()
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
