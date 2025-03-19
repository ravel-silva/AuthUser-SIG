using Microsoft.AspNetCore.Authorization;

namespace UserAuth.Authorization
{
    public class NivelDeAcesso : IAuthorizationRequirement
    {
        public NivelDeAcesso(string nivelDeAcesso)
        {
            nivel = nivelDeAcesso;
        }
        public string nivel { get; set; }
    }
}
