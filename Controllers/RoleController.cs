using Microsoft.AspNetCore.Mvc;
using UserAuth.Data.Dtos;
using UserAuth.Services;

namespace UserAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private RoleService _roleService;
        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
            var result = await _roleService.CreateRole(dto);
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddRoleUser(CreateRoleUserDto dto)
        {
            var result = await _roleService.AddRoleUser(dto);
            return Ok(result);
        }
        [HttpGet("view")]
        public async Task<IActionResult> ViewRoles()
        {
            var result = await _roleService.GetRolesUser();
            return Ok(result);
        }
    }
}
