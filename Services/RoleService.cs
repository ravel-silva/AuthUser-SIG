using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        //criação das roles
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
        //adicionar role ao usuário
        public async Task<IActionResult> AddRoleUser(CreateRoleUserDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.PrefixoUsuario == dto.prefixoUsuario);
            if (user == null)
            {
                return new BadRequestObjectResult("Usuário não cadastrado");
            }
            var RolesExiste = await _userManager.GetRolesAsync(user);
            var rolesToAdd = dto.RoleName.Except(RolesExiste);
            if (!rolesToAdd.Any())
            {
                return new BadRequestObjectResult("Usuário já possui as roles especificadas");
            }

            IdentityResult resultRole = await _userManager.AddToRolesAsync(user, rolesToAdd.ToList());
            if (!resultRole.Succeeded)
            {
                return new BadRequestObjectResult("Erro ao adicionar a role" + string.Join(", ", resultRole.Errors.Select(erro => erro.Description)));
            }
            return new OkObjectResult($"Role '{string.Join(",", dto.RoleName)}' adicionada com sucesso ao usuário '{user.UserName}'");
        }
        // retornar a lista de usuarios com suas roles
        public async Task<IActionResult> GetRolesUser()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<ReadRoleDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
               
                usersWithRoles.Add(new ReadRoleDto
                {
                    Prexifo = user.PrefixoUsuario,
                    Role = string.Join(", ", roles)
                });
            }
            return new OkObjectResult(usersWithRoles);
        }
    }
}
