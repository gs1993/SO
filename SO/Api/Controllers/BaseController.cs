using Api.Utils;
using AutoMapper;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;

        public BaseController( UnitOfWork unitOfWork, IMapper mapper)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }


        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }
    }
}
