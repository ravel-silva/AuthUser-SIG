using Microsoft.AspNetCore.Mvc;
using UserAuth.Data.Dtos;
using UserAuth.Services;

namespace UserAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            await _service.CreateUser(dto);
            return Ok();
        }
    }
}
