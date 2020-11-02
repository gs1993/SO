using AutoMapper;
using Logic.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApp.Utils;

namespace WebApp.Controllers
{
    [EnableCors("MyPolicy")]
    public class BaseController : Controller
    {
        protected readonly UnitOfWork UnitOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
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
