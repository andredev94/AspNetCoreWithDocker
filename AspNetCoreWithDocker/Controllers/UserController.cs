using AspNetCoreWithDocker.Business.Rules.User;
using AspNetCoreWithDocker.Models.DataBaseModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWithDocker.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] Users user)
        {
            if (user == null) return BadRequest();
            return Ok(_userBusiness.FindByLogin(user));
        }
    }
}
