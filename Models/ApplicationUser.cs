using Microsoft.AspNetCore.Identity;

namespace TreineMais.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? NomeCompleto { get; set; }
        public int? Idade { get; set; }
        public string? Objetivo { get; set; }

        // Instrutor ou Aluno
        public string? TipoUsuario { get; set; }
    }
}