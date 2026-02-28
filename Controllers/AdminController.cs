using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TreineMais.Data;
using TreineMais.Models;
using TreineMais.ViewModels;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Alunos()
        {
            var alunos = await _context.Users
                .Where(u => u.TipoUsuario == "Aluno")
                .ToListAsync();

            return View(alunos);
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

            // validações básicas (com retorno cedo => sem warnings)
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

            // evita duplicar e-mail
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
                TipoUsuario = "Aluno",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(novoAluno, senha);

            if (result.Succeeded)
            {
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

        public async Task<IActionResult> Treinos()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge(); // força login se algo estiver errado

            var alunos = await _context.Users
                .Where(u => u.TipoUsuario == "Aluno")
                .ToListAsync();

            ViewBag.Alunos = alunos;

            var treinos = await _context.Treinos
                .Where(t => t.InstrutorId == user.Id)
                .Include(t => t.Aluno)
                .ToListAsync();

            return View(treinos);
        }

        public async Task<IActionResult> CriarTreino()
        {
            var alunos = await _context.Users
                .Where(u => u.TipoUsuario == "Aluno")
                .ToListAsync();

            var viewModel = new CriarTreinoViewModel
            {
                Alunos = alunos
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarTreino(CriarTreinoViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                model.Alunos = await _context.Users
                    .Where(u => u.TipoUsuario == "Aluno")
                    .ToListAsync();

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