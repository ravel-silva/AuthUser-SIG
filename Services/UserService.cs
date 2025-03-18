using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data.Dtos;
using UserAuth.Model;

namespace UserAuth.Services
{
    public class UserService
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private SignInManager<User> _singInManager;

        public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> singInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _singInManager = singInManager;
        }

        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            string prefixo = GetUserPrefix(dto.NivelDeAcesso); // define o prefixo com base no nivel de acesso
            int userCount = await _userManager.Users
                .Where(userPrefixo => userPrefixo.UserName.StartsWith(prefixo))
                .CountAsync();
            string usercode = $"{prefixo}{(userCount + 1).ToString("D3")}"; // cria o código do usuário

            user.UserName = usercode; // define o código do usuário

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult($"Usuário criado com sucesso: {user.UserName}");
            }
            return new BadRequestObjectResult(result.Errors.Select(erro => erro.Description));
        }


        // Função para definir o prefixo
        private string GetUserPrefix(string role)
        {
            return role switch
            {
                "Admin" => "ADM",
                "Supervisor" => "SUP",
                _ => "USER" // Padrão para outros usuários
            };
        }
    }
}