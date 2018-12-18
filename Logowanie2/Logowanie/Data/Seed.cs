using Logowanie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Logowanie.Data
{
    public static class Seed
    {
       
        public static async Task CreateUserRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;


            string[] roleNames = { "Pacjent", "Recepcjonistka", "Lekarz" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            ApplicationUser user = await UserManager.FindByEmailAsync("prywatna_przychodnia@adres.pl");
            var User = new ApplicationUser();
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            IdentityRole adminrole = new IdentityRole();
            adminrole.Name = "Admin";

            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(adminrole);

                await RoleManager.AddClaimAsync(adminrole, new Claim("Can add roles", "add.role"));
                await RoleManager.AddClaimAsync(adminrole, new Claim("Can delete roles", "delete.role"));
                await RoleManager.AddClaimAsync(adminrole, new Claim("Can edit roles", "edit.role"));
                //create the roles and seed them to the database

           


            }


            await UserManager.AddToRoleAsync(user, "Admin");

            if (!roleCheck)
            {
                await UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
            }

        }
    }
}
