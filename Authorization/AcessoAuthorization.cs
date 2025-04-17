using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserAuth.Model;
using Microsoft.Extensions.Logging;

namespace UserAuth.Authorization
{
    public class AcessoAuthorization : AuthorizationHandler<NivelDeAcesso>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AcessoAuthorization> _logger;

        public AcessoAuthorization(UserManager<User> userManager, ILogger<AcessoAuthorization> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NivelDeAcesso requirement)
        {
            // Primeiro, verificamos os claims para garantir que o Id do usuário esteja presente.
            var idClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
            {
                _logger.LogWarning("⚠️ Claim do ID do usuário não encontrada.");
                context.Fail(); // Falha na autorização, pois o ID do usuário não foi encontrado
                return;
            }

            _logger.LogInformation($"[DEBUG] ClaimTypes.NameIdentifier: {idClaim}");

            // Agora buscamos o usuário na base de dados usando o Id.
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                _logger.LogWarning("⚠️ Usuário não encontrado com o ID: {idClaim}");
                context.Fail(); // Falha na autorização, pois o usuário não foi encontrado
                return;
            }

            _logger.LogInformation("Usuário encontrado: {userName}", user.UserName);

            // Agora buscamos os roles do usuário.
            var roles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation($"Roles do usuário: {string.Join(", ", roles)}");

            // Verificando se o usuário tem o role necessário para passar a autorização.
            if (roles.Contains(requirement.nivel))
            {
                _logger.LogInformation("O usuário tem o role adequado. Autorizado!");
                context.Succeed(requirement); // Sucesso na autorização
            }
            else
            {
                _logger.LogWarning("O usuário não tem o role adequado. Falha na autorização.");
                context.Fail(); // Falha na autorização, pois o usuário não tem o role necessário
            }
        }
    }
}
