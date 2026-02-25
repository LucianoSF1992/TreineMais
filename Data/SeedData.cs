using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TreineMais.Models;

namespace TreineMais.Data
{
    public static class SeedData
    {
        public static async Task Inicializar(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Criar Instrutor
            if (await userManager.FindByEmailAsync("admin@academia.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@academia.com",
                    Email = "admin@academia.com",
                    NomeCompleto = "Instrutor Admin",
                    TipoUsuario = "Instrutor",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "admin");
            }

            // Criar Aluno
            if (await userManager.FindByEmailAsync("aluno@academia.com") == null)
            {
                var aluno = new ApplicationUser
                {
                    UserName = "aluno@academia.com",
                    Email = "aluno@academia.com",
                    NomeCompleto = "Aluno Teste",
                    TipoUsuario = "Aluno",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(aluno, "aluno");
            }
        }
    }
}