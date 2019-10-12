using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dawn;
using Logic.Dtos;
using AutoMapper;
using Logic.Models;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repositories
{
    public class PostsRepository : Repository<Posts>
    {
        protected readonly IMapper Mapper;
        public PostsRepository(UnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            Mapper = mapper;
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
                    new PostListDto(p.Id, p.Title, p.Body, p.AnswerCount, p.CommentCount, p.CreationDate, p.Score, p.ViewCount, p.ClosedDate)
                )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<PostListDto>> GetLastest(int size)
        {
            Guard.Argument(size, nameof(size)).Positive();

            return await unitOfWork.Query<Posts>()
                .Where(p => !string.IsNullOrEmpty(p.Title))
                .OrderByDescending(p => p.Id)
                .Take(size)
                .Select(p => 
                    new PostListDto(p.Id, p.Title, p.Body, p.AnswerCount, p.CommentCount, p.CreationDate, p.Score, p.ViewCount, p.ClosedDate)
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

        public async Task<PostDetailsDto> GetWithAnswers(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();


            var post = await unitOfWork.Query<Posts>()
                .FirstOrDefaultAsync(p => p.Id == id);

            var comments = await unitOfWork.Query<Comments>()
                .Where(c => c.PostId == id)
                .ToListAsync();

            return new PostDetailsDto()
        }
    }
}
