using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Identity
{
    public class IdentityInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IdentityDataContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Roller veritabanından kontrol edilir
            var roles = await context.Roles.ToListAsync();
            if (!roles.Any(r => r.Name == "admin"))
            {
                var role = new ApplicationRole() { Name = "admin", Description = "admin rolü" };
                await roleManager.CreateAsync(role);
            }

            if (!roles.Any(r => r.Name == "user"))
            {
                var role = new ApplicationRole() { Name = "user", Description = "user rolü" };
                await roleManager.CreateAsync(role);
            }

            // Kullanıcılar veritabanından kontrol edilir
            var users = await context.Users.ToListAsync();
            if (!users.Any(u => u.UserName == "ebrarakcaseven"))
            {
                var user = new ApplicationUser()
                {
                    Name = "ebrar",
                    Surname = "akcaseven",
                    UserName = "ebrarakcaseven",
                    Email = "ebrarakcaseven@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                    await userManager.AddToRoleAsync(user, "user");
                }
            }

            if (!users.Any(u => u.UserName == "seymaakcaseven"))
            {
                var user = new ApplicationUser()
                {
                    Name = "seyma",
                    Surname = "akcaseven",
                    UserName = "seymaakcaseven",
                    Email = "seymaakcaseven@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
