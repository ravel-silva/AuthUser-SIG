using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuth.Model;
using Microsoft.AspNetCore.Identity;

namespace UserAuth.Services
{
    public class TokenService
    {
        private readonly UserManager<User> _userManager;

        public TokenService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(User user)
        {
            // Criando lista de claims básicas
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("PrefixoUsuario", user.PrefixoUsuario),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Matricula", user.Matricula.ToString())
            };

            // Obtendo todas as roles do usuário corretamente do banco
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Criando chave de segurança
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopásdfghjklçzxcvbnm7531522HJBHKNJLMKFDKGSSHSAEW"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            // Criando token JWT com claims e roles
            var token = new JwtSecurityToken(
                issuer: "meu-sistema",
                audience: "meus-clientes",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
