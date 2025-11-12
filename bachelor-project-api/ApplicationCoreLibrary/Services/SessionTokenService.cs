using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApplicationCoreLibrary.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCoreLibrary.Services
{
    public static class SessionTokenService
    {
        public static async Task ValidateSessionToken(TokenValidatedContext context)
        {
            var repository = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            if (context.Principal.HasClaim(c => c.Type.Equals(JwtRegisteredClaimNames.Jti, StringComparison.OrdinalIgnoreCase)))
            {
                var jti = context.Principal.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Jti, StringComparison.OrdinalIgnoreCase)).Value;
                // Check jti is still in store, and that the expiration of the jti is ok
                var tokenInStore = await repository.SessionToken.GetByJTI(jti);
                if (tokenInStore != null && tokenInStore.ExpirationDate > DateTime.UtcNow)
                {
                    return;
                }
            }

            context.Fail("");
        }
    }
}
