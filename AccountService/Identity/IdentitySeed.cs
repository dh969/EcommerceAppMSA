using AccountService.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AccountService.Identity
{
    public class IdentitySeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            var user = new AppUser
            {
                DisplayName = "admin",
                Email = "admin3@ecommerce.com",
                UserName = "admin3@ecommerce.com",
                PhoneNumber = "1234567890"
            };
            var user1 = new AppUser
            {
                DisplayName = "Dhruv",
                Email = "dhruvchawla0101@gmail.com",
                UserName = "dhruvchawla0101@gmail.com",
                PhoneNumber = "1234567890"

            };
            if (await userManager.FindByEmailAsync(user1.Email) == null)
            {
                await userManager.CreateAsync(user1, "Dhruv@22102001");
                if (!await roleManager.RoleExistsAsync("user"))
                {
                    var role = new IdentityRole();
                    role.Name = "user";
                    await roleManager.CreateAsync(role);
                }
                await userManager.AddToRoleAsync(user1, "user");
            }
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "Dhruv@22102001");
                if (!await roleManager.RoleExistsAsync("admin"))
                {
                    var role = new IdentityRole();
                    role.Name = "admin";
                    await roleManager.CreateAsync(role);
                }
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
