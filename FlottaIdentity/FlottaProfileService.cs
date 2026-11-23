using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using FlottaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlottaIdentity
{
    public class FlottaProfileService : IProfileService
    {

        FlottaContext _context;
        public FlottaProfileService(FlottaContext context) {
            _context = context;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // A ValidateAsync-ben megadott 'subject' (user.Id) kiolvasása
            var sub = context.Subject.GetSubjectId();
            //var sub = context.Subject
            if (!int.TryParse(sub, out int userId)) return;

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return;

            // Ezek a claim-ek kerülnek bele a JWT tokenbe
            var claims = new List<Claim>
{
    new Claim("name", user.LoginName),
    new Claim("role", user.Status)
};

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            // Ellenőrzi, hogy a felhasználó még aktív-e (pl. nincs-e kitiltva)
            var sub = context.Subject.GetSubjectId();
            if (int.TryParse(sub, out int userId))
            {
                var user = await _context.Users.FindAsync(userId);
                context.IsActive = user != null && user.Active;
            }
            else
            {
                context.IsActive = false;
            }
        }


    }
}
