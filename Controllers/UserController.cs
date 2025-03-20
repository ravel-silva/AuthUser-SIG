using Microsoft.AspNetCore.Authorization;
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
           var result = await _service.CreateUser(dto);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var result = await _service.Login(dto);
            return Ok(result);
        }
        [HttpGet]
        [Authorize(policy: "RequireAdmin")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
        [HttpGet("teste")]
        [Authorize(policy: "RequireStandardUser")]
        public IActionResult Get2()
        {
            return Ok("ok");
        }
        [HttpGet("teste2")]
        [Authorize(policy: "RequireSupervisor")]
        public IActionResult Get3()
        {
            return Ok("ok");
        }
    }
}
