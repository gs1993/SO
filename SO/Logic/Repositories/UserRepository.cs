using Logic.Dtos;
using Logic.Models;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repositories
{
    public class UserRepository : Repository<Users>
    {
        public UserRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IReadOnlyList<LastUserDto>> GetLastUsers(int count)
        {
            if (count <= 0) // TODO: Add Guard
                throw new ArgumentException($"Invalid argument count = {count}");

            return await unitOfWork.Query<Users>()
                .OrderByDescending(u => u.CreationDate)
                .Take(count)
                .Select(u => new LastUserDto { CreationDate = u.CreationDate, DisplayName = u.DisplayName })
                .ToListAsync();
        }

        public async Task<Result> PermaBanUser(int id)
        {
            if (id <= 0) 
                throw new ArgumentException($"Invalid argument id: {id}");

            var user = await unitOfWork.Query<Users>().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return Result.Fail($"There is no users with id: {id}");

            unitOfWork.Delete(user);
            unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
