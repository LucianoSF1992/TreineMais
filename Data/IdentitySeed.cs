using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TreineMais.Models;

namespace TreineMais.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Roles
            var roles = new[] { "Admin", "Instrutor", "Aluno" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Admin
            await EnsureUserAsync(
                userManager,
                email: "admin@treinemais.com",
                password: "Admin@123",
                role: "Admin",
                tipoUsuario: "Admin"
            );

            // Instrutor
            await EnsureUserAsync(
                userManager,
                email: "instrutor@treinemais.com",
                password: "Instrutor@123",
                role: "Instrutor",
                tipoUsuario: "Instrutor"
            );

            // Aluno
            await EnsureUserAsync(
                userManager,
                email: "aluno@treinemais.com",
                password: "Aluno@123",
                role: "Aluno",
                tipoUsuario: "Aluno"
            );
        }

        private static async Task EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string role,
            string tipoUsuario)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    TipoUsuario = tipoUsuario
                };

                var create = await userManager.CreateAsync(user, password);
                if (!create.Succeeded)
                {
                    var errors = string.Join(" | ", create.Errors.Select(e => e.Description));
                    throw new Exception($"Falha ao criar usuário {email}: {errors}");
                }
            }
            else
            {
                // garante TipoUsuario
                if (string.IsNullOrWhiteSpace(user.TipoUsuario) || user.TipoUsuario != tipoUsuario)
                {
                    user.TipoUsuario = tipoUsuario;
                    await userManager.UpdateAsync(user);
                }

                // 🔑 força reset da senha
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, token, password);
            }

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);
        }
    }
}