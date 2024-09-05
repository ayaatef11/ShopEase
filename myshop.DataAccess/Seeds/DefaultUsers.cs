using Microsoft.AspNetCore.Identity;
using myshop.DataAccess.Data.Constants;
using System.Security.Claims;

namespace myshop.DataAccess.Seeds
{
    public  static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "basicuser@domain.com",
                Email = "basicuser@domain.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin@domain.com",
                Email = "superadmin@domain.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRolesAsync(defaultUser, [Roles.Basic.ToString(), Roles.Admin.ToString(), Roles.SuperAdmin.ToString()]);
            }
            await roleManger.SeedClaimsForSuperUser();
        }

        private static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddPermissionClaims(adminRole, "Products");
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }

    }
}
