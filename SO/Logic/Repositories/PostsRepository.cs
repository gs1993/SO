using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Dtos;
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

        public async Task<IReadOnlyList<PostListDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Query<Posts>()
                .Where(p => !string.IsNullOrEmpty(p.Title))
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => 
                    new PostListDto(p.Title, p.AnswerCount, p.CommentCount, p.CreationDate, p.Score, p.ViewCount, p.ClosedDate)
                )
                .ToListAsync();
        }
    }
}
