﻿using Logic.Models.Users;

namespace Logic.Repositories
{
    public class UserRepository : Repository<Users>
    {
        //public UserRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        //{
        //}

        //public async Task<IReadOnlyList<LastUserDto>> GetLastUsers(int count)
        //{
        //    Guard.Argument(count, nameof(count)).Positive();

        //    return await unitOfWork.Query<Users>()
        //        .OrderByDescending(u => u.CreationDate)
        //        .Take(count)
        //        .Select(u => new LastUserDto { Id = u.Id, DisplayName = u.DisplayName })
        //        .ToListAsync();
        //}

        //public async Task<LastUserDto> GetLastUsersWithIndex(int index)
        //{
        //    Guard.Argument(index, nameof(index)).Positive();

        //    return await unitOfWork.Query<Users>()
        //        .OrderByDescending(u => u.CreationDate)
        //        .Skip(index)
        //        .Select(u => new LastUserDto { Id = u.Id, DisplayName = u.DisplayName })
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<Result> PermaBanUser(int id)
        //{
        //    Guard.Argument(id, nameof(id)).Positive();

        //    var user = await unitOfWork.Query<Users>().FirstOrDefaultAsync(u => u.Id == id);
        //    if (user == null)
        //        return Result.Failure($"There is no users with id: {id}");

        //    unitOfWork.Delete(user);
        //    unitOfWork.Commit();

        //    return Result.Success();
        //}

        //public Task<int> GetCreatedPostCount(int id)
        //{
        //    Guard.Argument(id, nameof(id)).Positive();

        //    return unitOfWork.Query<Posts>()
        //        .CountAsync(p => p.OwnerUserId == id);
        //}
    }
}
