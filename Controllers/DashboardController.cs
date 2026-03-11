using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreineMais.Data;
using TreineMais.Models;
using TreineMais.ViewModels.Dashboard;

namespace TreineMais.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public DashboardController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction(nameof(Admin));

            if (User.IsInRole("Instrutor"))
                return RedirectToAction(nameof(Instrutor));

            if (User.IsInRole("Aluno"))
                return RedirectToAction(nameof(Aluno));

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var totalUsuarios = await _context.Users.CountAsync();

            var alunos = await _userManager.GetUsersInRoleAsync("Aluno");
            var instrutores = await _userManager.GetUsersInRoleAsync("Instrutor");

            var vm = new AdminDashboardViewModel
            {
                TotalUsuarios = totalUsuarios,
                TotalAlunos = alunos.Count,
                TotalInstrutores = instrutores.Count,
                TotalTreinos = await _context.Treinos.CountAsync()
            };

            return View(vm);
        }

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> Instrutor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // últimos treinos (lista rápida)
            var ultimosTreinos = await _context.Treinos
                .Where(t => t.InstrutorId == user.Id)
                .Include(t => t.Aluno)
                .OrderByDescending(t => t.Id)
                .Take(10)
                .Select(t => new TreinoResumoViewModel
                {
                    TreinoId = t.Id,
                    Nome = t.Nome,
                    DiaSemana = t.DiaSemana,
                    AlunoNome = t.Aluno != null
                        ? (t.Aluno.NomeCompleto ?? t.Aluno.Email ?? "—")
                        : "—",
                    Concluido = t.Concluido
                })
                .ToListAsync();

            // resumo por aluno (somente alunos do instrutor)
            var resumoAlunos = await _context.Treinos
                .Where(t => t.InstrutorId == user.Id)
                .Include(t => t.Aluno)
                .GroupBy(t => new
                {
                    t.AlunoId,
                    Nome = t.Aluno != null ? (t.Aluno.NomeCompleto ?? "") : "",
                    Email = t.Aluno != null ? (t.Aluno.Email ?? "") : ""
                })
                .Select(g => new AlunoResumoViewModel
                {
                    AlunoId = g.Key.AlunoId,
                    Nome = string.IsNullOrWhiteSpace(g.Key.Nome) ? "Aluno" : g.Key.Nome,
                    Email = g.Key.Email,
                    TreinosDoAluno = g.Count(),
                    Concluidos = g.Count(x => x.Concluido),
                    Pendentes = g.Count(x => !x.Concluido)
                })
                .OrderByDescending(x => x.TreinosDoAluno)
                .Take(8)
                .ToListAsync();

            var totalTreinos = await _context.Treinos.CountAsync(t => t.InstrutorId == user.Id);

            var totalAlunos = await _context.Users
                .CountAsync(u => u.TipoUsuario == "Aluno" && u.InstrutorId == user.Id);

            // ✅ NOVO: status geral p/ gráfico
            var concluidos = await _context.Treinos.CountAsync(t => t.InstrutorId == user.Id && t.Concluido);
            var pendentes = await _context.Treinos.CountAsync(t => t.InstrutorId == user.Id && !t.Concluido);

            // ✅ NOVO: treinos por dia da semana p/ gráfico
            var porDia = await _context.Treinos
                .Where(t => t.InstrutorId == user.Id)
                .GroupBy(t => t.DiaSemana)
                .Select(g => new { Dia = g.Key, Qtde = g.Count() })
                .ToListAsync();

            var treinosPorDiaSemana = porDia
                .Where(x => !string.IsNullOrWhiteSpace(x.Dia))
                .ToDictionary(x => x.Dia, x => x.Qtde);

            var vm = new InstrutorDashboardViewModel
            {
                TotalTreinos = totalTreinos,
                TotalAlunos = totalAlunos,
                MeusAlunos = resumoAlunos,
                UltimosTreinos = ultimosTreinos,

                Concluidos = concluidos,
                Pendentes = pendentes,
                TreinosPorDiaSemana = treinosPorDiaSemana
            };

            return View(vm);
        }

        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Aluno()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var meusTreinos = await _context.Treinos
                .Where(t => t.AlunoId == user.Id)
                .Include(t => t.Instrutor)
                .OrderByDescending(t => t.Id)
                .ToListAsync();

            var vm = new AlunoDashboardViewModel
            {
                TotalTreinos = meusTreinos.Count,
                MeusTreinos = meusTreinos,

                Concluidos = meusTreinos.Count(t => t.Concluido),
                Pendentes = meusTreinos.Count(t => !t.Concluido)
            };

            return View(vm);
        }
    }
}