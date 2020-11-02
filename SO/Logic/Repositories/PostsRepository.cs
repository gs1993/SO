using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dawn;
using Logic.Dtos;
using Logic.Models;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Dapper;

namespace Logic.Repositories
{
    public class PostsRepository : Repository<Posts>
    {
        public PostsRepository(UnitOfWork unitOfWork, QueriesConnectionString connectionString) : base(unitOfWork, connectionString)
        {
        }

        public async Task<IReadOnlyList<PostListDto>> GetPageAsync(int pageNumber, int pageSize)
        {
            Guard.Argument(pageNumber, nameof(pageNumber)).Positive();
            Guard.Argument(pageSize, nameof(pageSize)).Positive();

            int offset = pageNumber * pageSize;

            string sql = @"
                    SELECT [p].[Id], [p].[Title], [p].[Body], [p].[AnswerCount], [p].[CommentCount], [p].[CreationDate], [p].[Score], 
                           [p].[ViewCount], [p].[ClosedDate]
                    FROM [Posts] AS [p]
                    WHERE [p].[Title] IS NOT NULL
                    ORDER BY p.Id
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY";

            using (SqlConnection connection = new SqlConnection(_connectionString.Value))
            {
                var posts = await connection.QueryAsync<PostListDto>(sql, new
                {
                    Offset = offset,
                    PageSize = pageSize
                });

                return posts.ToList();
            }
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

        public async Task<Posts> GetWithAnswers(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();
            
            var post = await unitOfWork.Query<Posts>()
                .FirstOrDefaultAsync(p => p.Id == id);

            var comments = await unitOfWork.Query<Comments>()
                .Where(c => c.PostId == id)
                .ToListAsync();

            post.Comments = comments; // TODO: Fix
            return post;
        }
    }
}
