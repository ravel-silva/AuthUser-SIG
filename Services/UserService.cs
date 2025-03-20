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
        private TokenService _tokenService;

        public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> singInManager, TokenService tokenService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _singInManager = singInManager;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            User user = _mapper.Map<User>(dto);
            string prefixo = GetUserPrefix(dto.NivelDeAcesso); // define o prefixo com base no nivel de acesso
            int userCount = await _userManager.Users
                .Where(userPrefixo => userPrefixo.PrefixoUsuario.StartsWith(prefixo))
                .CountAsync();
            string usercode = $"{prefixo}{(userCount + 1).ToString("D3")}"; // cria o código do usuário

            user.UserName = dto.Username.Replace(" ", "_");
            user.PrefixoUsuario = usercode;

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult($"Usuário criado com sucesso: {user.PrefixoUsuario}");
            }
            return new BadRequestObjectResult(result.Errors.Select(erro => erro.Description));
        }

        public async Task <string> Login(LoginUserDto dto)
        {
            var user = await _singInManager
                                       .UserManager
                                       .Users
                                       .FirstOrDefaultAsync(user => user.PrefixoUsuario == dto.PrefixoUsuario.ToUpper());
            if (user == null)
            {
                throw new ApplicationException("Usuário não cadastrado");
            }

            var result = await _singInManager.PasswordSignInAsync(user.UserName, dto.Password, false, false);

            if (!result.Succeeded)
            {
                throw new ApplicationException("Usuário ou senha inválidos");
            }

            
            var token = _tokenService.GenerateToken(user);
            return token;
        }

        // Função para definir o prefixo
        private string GetUserPrefix(string role)
        {
            return role switch
            {
                "Administrador" => "ADM",
                "Supervisor" => "SUP",
                "Padrao" => "USER"
            };
        }
    }
}