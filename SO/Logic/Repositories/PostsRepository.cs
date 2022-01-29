using Logic.Models;

namespace Logic.Repositories
{
    //public class PostsRepository : Repository<Posts>
    //{
    //    //protected readonly IMapper Mapper;
    //    //public PostsRepository(UnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    //    //{
    //    //    Mapper = mapper;
    //    //}

    //    //public async Task<IReadOnlyList<PostListDto>> GetPageAsync(int pageNumber, int pageSize)
    //    //{
    //    //    Guard.Argument(pageNumber, nameof(pageNumber)).Positive();
    //    //    Guard.Argument(pageSize, nameof(pageSize)).Positive();

    //    //    return await unitOfWork.Query<Posts>()
    //    //        .Where(p => !string.IsNullOrEmpty(p.Title))
    //    //        .OrderBy(p => p.Id)
    //    //        .Skip((pageNumber - 1) * pageSize)
    //    //        .Take(pageSize)
    //    //        .Select(p => 
    //    //            new PostListDto(p.Id, p.Title, p.Body, p.AnswerCount, p.CommentCount, p.CreationDate, p.Score, p.ViewCount, p.ClosedDate)
    //    //        )
    //    //        .ToListAsync();
    //    //}

    //    //public async Task<IReadOnlyList<PostListDto>> GetLastest(int size)
    //    //{
    //    //    Guard.Argument(size, nameof(size)).Positive();

    //    //    return await unitOfWork.Query<Posts>()
    //    //        .Where(p => !string.IsNullOrEmpty(p.Title))
    //    //        .OrderByDescending(p => p.Id)
    //    //        .Take(size)
    //    //        .Select(p => 
    //    //            new PostListDto(p.Id, p.Title, p.Body, p.AnswerCount, p.CommentCount, p.CreationDate, p.Score, p.ViewCount, p.ClosedDate)
    //    //        )
    //    //        .ToListAsync();
    //    //}

    //    //public async Task<Result> Delete(int id)
    //    //{
    //    //    Guard.Argument(id, nameof(id)).Positive();

    //    //    var post = await unitOfWork.Query<Posts>()
    //    //        .FirstOrDefaultAsync(p => p.Id == id);
    //    //    if(post == null)
    //    //        return Result.Failure($"There is no posts with id: {id}");

    //    //    unitOfWork.Delete(post);
    //    //    return Result.Success();
    //    //}

    //    //public async Task<Posts> GetWithAnswers(int id)
    //    //{
    //    //    Guard.Argument(id, nameof(id)).Positive();
            
    //    //    var post = await unitOfWork.Query<Posts>()
    //    //        .FirstOrDefaultAsync(p => p.Id == id);

    //    //    var comments = await unitOfWork.Query<Comments>()
    //    //        .Where(c => c.PostId == id)
    //    //        .ToListAsync();

    //    //    post.Comments = comments; // TODO: Fix
    //    //    return post;
    //    //}
    //}
}
