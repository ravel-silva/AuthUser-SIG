using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Data.Dtos;
using UserAuth.Model;

namespace UserAuth.Services
{

    public class RoleService
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.PrefixoRole))
            {
                return new BadRequestObjectResult("Role ja existente");
            }
            IdentityRole role = _mapper.Map<IdentityRole>(dto);
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult("Erro ao criar a role" + string.Join(", ", result.Errors.Select(erro => erro.Description)));
            }
            return new OkObjectResult($"Role '{role}' criada com sucesso");
        }
        public async Task<IActionResult> AddRoleUser(CreateRoleUserDto dto)
        {
            if (!await _roleManager.RoleExistsAsync(dto.RoleName))
            {
                return new BadRequestObjectResult("Role não existente");
            }
            var result = _userManager.Users.FirstOrDefault(user => user.PrefixoUsuario == dto.prefixoUsuario);
            if (result == null){
                return new BadRequestObjectResult("Usuário não cadastrado");
            }
            IdentityRole AddRole = _mapper.Map<IdentityRole>(dto);
            IdentityResult resultRole = await _userManager.AddToRoleAsync(result, AddRole.Name);
            if (!resultRole.Succeeded)
            {
                return new BadRequestObjectResult("Erro ao adicionar a role" + string.Join(", ", resultRole.Errors.Select(erro => erro.Description)));
            }
            return new OkObjectResult($"Role '{AddRole.Name}' adicionada com sucesso ao usuário '{result.PrefixoUsuario}'");
        }
    }
}
