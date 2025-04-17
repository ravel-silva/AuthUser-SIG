using Microsoft.AspNetCore.Authorization;

namespace UserAuth.Authorization
{
    public class NivelDeAcesso : IAuthorizationRequirement
    {
        public string nivel { get; set; }
        public NivelDeAcesso(string nivelDeAcesso)
        {
            this.nivel = nivelDeAcesso;
        }
    }
}
