
using FlottaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlottaIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<FlottaContext>(options => {
                string connstr = builder.Configuration.GetConnectionString("FlottaConnection");
                options.UseMySql(connstr, ServerVersion.AutoDetect(connstr));
            });
            builder.Services.AddIdentityServer()
    .       AddInMemoryIdentityResources(Config.IdentityResources)
    .       AddInMemoryApiScopes(Config.ApiScopes)
    .       AddInMemoryClients(Config.Clients)
    .       AddResourceOwnerValidator<FlottaPasswordValidator>() // <--- Saját Argon2 ellenõrzés
    .       AddProfileService<FlottaProfileService>()             // <--- Saját claim-ek betöltése
    .       AddDeveloperSigningCredential();



            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
