using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace UserAuth.Authorization
{
    public class AcessoAuthorization : AuthorizationHandler<NivelDeAcesso>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NivelDeAcesso requirement)
        {

            var nivelDeAcessoClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);


            if (nivelDeAcessoClaim == null)
            {
                Console.WriteLine("❌ Nenhuma claim de nível de acesso encontrada.");
                return Task.CompletedTask;
            }
            if (nivelDeAcessoClaim.Value == "Administrador")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            // personalizando o nivel de supervisores
            var acessosPermitidosParaSupervisores = new[]
            {
                "Supervisor",
                "Padrao",
            };

            if (context.User.IsInRole("Supervisor") &&
                acessosPermitidosParaSupervisores.Contains(requirement.nivel))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            if (nivelDeAcessoClaim.Value == requirement.nivel)
            {
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine($"❌ Acesso negado! Esperado: {requirement.nivel}, recebido: {nivelDeAcessoClaim.Value}");
            }
            return Task.CompletedTask;
        }
    }
}
