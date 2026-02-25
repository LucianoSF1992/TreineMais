using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TreineMais.Models;

namespace TreineMais.Data
{
    public static class SeedData
    {
        public static async Task Inicializar(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 🔹 Criar Roles
            string[] roles = { "Admin", "Instrutor", "Aluno" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 🔹 Criar Admin padrão
            var adminEmail = "admin@treinemais.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // 🔹 Criar Instrutor padrão
            var instrutorEmail = "instrutor@treinemais.com";

            if (await userManager.FindByEmailAsync(instrutorEmail) == null)
            {
                var instrutor = new ApplicationUser
                {
                    UserName = instrutorEmail,
                    Email = instrutorEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(instrutor, "Instrutor123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(instrutor, "Instrutor");
                }
            }
        }
    }
}