using AutoMapper;
using Dawn;
using Logic.Dtos;
using Logic.Models.Users;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private UserRepository _userRepository;

        public UsersController(UnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _userRepository = new UserRepository(unitOfWork);
        }


        [HttpGet]
        [Route("GetLast")]
        public async Task<IActionResult> GetLastCreated(int size = 10)
        {
            Guard.Argument(size, nameof(size)).Positive();

            var lastUsersDto = await _userRepository.GetLastUsers(size);
            if (lastUsersDto == null)
                return Error("Not Found");

            return Ok(lastUsersDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Error("Not Found");

            int createdPostCount = await _userRepository.GetCreatedPostCount(id);

            var createdPostCountResult = user.SetCreatedPostCount(createdPostCount);
            if (createdPostCountResult.IsFailure)
                return Error(createdPostCountResult.Error);

            var userDto = Mapper.Map<UserDetailsDto>(user);
            return Ok(userDto);
        }

        [HttpGet]
        [Route("GetLastCreatedByIndex/{index}")]
        public async Task<IActionResult> GetLastCreatedByIndex(int index)
        {
            Guard.Argument(index, nameof(index)).Positive();

            var lastUserDto = await _userRepository.GetLastUsersWithIndex(index);
            if (lastUserDto == null)
                return Error("Not Found");

            return Ok(lastUserDto);
        }

        [HttpDelete]
        [Route("PermaBan/{id}")]
        public async Task<IActionResult> PermaBan(int id)
        {
            Guard.Argument(id, nameof(id)).Positive();

            var result = await _userRepository.PermaBanUser(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
