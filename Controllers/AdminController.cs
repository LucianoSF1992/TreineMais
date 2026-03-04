using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreineMais.Data;
using TreineMais.Models;
using TreineMais.ViewModels;

namespace TreineMais.Controllers
{
    [Authorize(Roles = "Admin,Instrutor")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ===================== ALUNOS =====================

        public async Task<IActionResult> Alunos()
        {
            var alunos = await _userManager.GetUsersInRoleAsync("Aluno");
            return View(alunos.ToList());
        }

        public IActionResult CriarAluno()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarAluno(ApplicationUser? model, string? senha)
        {
            if (model is null)
                return BadRequest();

            var email = model.Email?.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(nameof(model.Email), "E-mail é obrigatório.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                ModelState.AddModelError(nameof(senha), "Senha é obrigatória.");
                return View(model);
            }

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Já existe um usuário com este e-mail.");
                return View(model);
            }

            var novoAluno = new ApplicationUser
            {
                UserName = email,
                Email = email,
                NomeCompleto = model.NomeCompleto,
                Idade = model.Idade,
                Objetivo = model.Objetivo,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(novoAluno, senha);

            if (result.Succeeded)
            {
                // ✅ fonte de verdade = Role
                await _userManager.AddToRoleAsync(novoAluno, "Aluno");
                return RedirectToAction(nameof(Alunos));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirAluno(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(Alunos));

            var aluno = await _userManager.FindByIdAsync(id);

            if (aluno != null)
                await _userManager.DeleteAsync(aluno);

            return RedirectToAction(nameof(Alunos));
        }

        // ===================== TREINOS (INSTRUTOR) =====================

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> Treinos()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var alunos = await _userManager.GetUsersInRoleAsync("Aluno");
            ViewBag.Alunos = alunos.ToList();

            var treinos = await _context.Treinos
                .Where(t => t.InstrutorId == user.Id)
                .Include(t => t.Aluno)
                .ToListAsync();

            return View(treinos);
        }

        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> CriarTreino()
        {
            var alunos = await _userManager.GetUsersInRoleAsync("Aluno");

            var viewModel = new CriarTreinoViewModel
            {
                Alunos = alunos.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instrutor")]
        public async Task<IActionResult> CriarTreino(CriarTreinoViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                var alunos = await _userManager.GetUsersInRoleAsync("Aluno");
                model.Alunos = alunos.ToList();
                return View(model);
            }

            // (Opcional) valida se o alunoId pertence mesmo a um usuário na role Aluno
            var aluno = await _userManager.FindByIdAsync(model.AlunoId);
            if (aluno == null || !await _userManager.IsInRoleAsync(aluno, "Aluno"))
            {
                ModelState.AddModelError(nameof(model.AlunoId), "Selecione um aluno válido.");
                var alunos = await _userManager.GetUsersInRoleAsync("Aluno");
                model.Alunos = alunos.ToList();
                return View(model);
            }

            var treino = new Treino
            {
                Nome = model.NomeTreino,
                DiaSemana = model.DiaSemana,
                AlunoId = model.AlunoId,
                InstrutorId = user.Id
            };

            _context.Treinos.Add(treino);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Treinos));
        }
    }
}