using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuth.Model;

namespace UserAuth.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim ("PrefixoUsuario", user.PrefixoUsuario),
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim ("Matricula", user.Matricula.ToString()),
                new Claim (ClaimTypes.Role, user.NivelDeAcesso),

            };
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopásdfghjklçzxcvbnm7531522HJBHKNJLMKFDKGSSHSAEW"));
            var crendenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: crendenciais
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
