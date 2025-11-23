using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CharityHub.Models;

namespace CharityHub.Models
{
    public static class IdentitySeedData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            // Створюємо базу даних, якщо її немає
            context.Database.EnsureCreated();

            // Створюємо ролі, якщо вони не існують
            string[] roles = { "Admin", "User" };
            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Створюємо адміністратора за замовчуванням, якщо його немає
            string adminEmail = "admin@charityhub.com";
            string adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                AppUser admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User"
                };

                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}

