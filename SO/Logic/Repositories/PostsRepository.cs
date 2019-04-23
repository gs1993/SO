using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dawn;
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
            Guard.Argument(pageNumber, nameof(pageNumber)).Positive();
            Guard.Argument(pageSize, nameof(pageSize)).Positive();

            return await unitOfWork.Query<Posts>()
                .Where(p => !string.IsNullOrEmpty(p.Title))
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => 
                    new PostListDto(p.Id, p.Title, p.AnswerCount, p.CommentCount, p.CreationDate, p.Score, p.ViewCount, p.ClosedDate)
                )
                .ToListAsync();
        }

        public async Task<Result> Delete(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var post = await unitOfWork.Query<Posts>()
                .FirstOrDefaultAsync(p => p.Id == id);
            if(post == null)
                return Result.Fail($"There is no posts with id: {id}");

            unitOfWork.Delete(post);
            return Result.Ok();
        }
    }
}
