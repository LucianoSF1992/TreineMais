using Microsoft.AspNetCore.Identity;
using TreineMais.Models;

namespace TreineMais.Data;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = ["Admin", "Instrutor", "Aluno"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        await EnsureUserWithRoleAsync(userManager, "admin@treinemais.com", "Admin@12345", "Admin");
        await EnsureUserWithRoleAsync(userManager, "instrutor@treinemais.com", "Instrutor@12345", "Instrutor");
        await EnsureUserWithRoleAsync(userManager, "aluno@treinemais.com", "Aluno@12345", "Aluno");
    }

    private static async Task EnsureUserWithRoleAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                throw new Exception($"Falha ao criar {email}: {string.Join(" | ", createResult.Errors.Select(e => e.Description))}");
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var addRoleResult = await userManager.AddToRoleAsync(user, role);
            if (!addRoleResult.Succeeded)
                throw new Exception($"Falha ao adicionar role {role} em {email}: {string.Join(" | ", addRoleResult.Errors.Select(e => e.Description))}");
        }
    }
}