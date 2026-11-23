using Argon2Sharp;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using FlottaAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace FlottaIdentity
{
    public class FlottaPasswordValidator : IResourceOwnerPasswordValidator
    {
        FlottaContext _context;
        public FlottaPasswordValidator(FlottaContext context){
            _context = context;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == context.UserName);
            if (user == null || !user.Active || !Argon2PhcFormat.VerifyPhcString(context.Password, user.Hash).IsValid)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant, "Hibás felhasználónév vagy jelszó. ");
                return;

            }
            context.Result = new GrantValidationResult(
                subject: user.Id.ToString(),
                authenticationMethod: "custom_argon2");
            
        }

    };
}
