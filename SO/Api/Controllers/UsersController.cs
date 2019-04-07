using AutoMapper;
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
            if (size < 1)
                return Error($"Parameter size cannot have value: {size}");

            var lastUsersDto = await _userRepository.GetLastUsers(size);
            if (lastUsersDto == null)
                return Error("Not Found");

            return Ok(lastUsersDto);
        }

        [HttpDelete]
        [Route("PermaBan")]
        public async Task<IActionResult> PermaBan(int id)
        {
            if (id < 1)
                return Error($"Parameter id cannot have value: {id}");

            var result = await _userRepository.PermaBanUser(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
