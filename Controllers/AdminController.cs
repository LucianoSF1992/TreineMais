using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TreineMais.Models;
using TreineMais.Data;
using Microsoft.EntityFrameworkCore;
using TreineMais.ViewModels;

namespace TreineMais.Controllers
{
    [Authorize]
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
            var user = await _userManager.GetUserAsync(User);

            if (user?.TipoUsuario != "Instrutor")
                return RedirectToAction("Index", "Home");

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
        public async Task<IActionResult> CriarAluno(ApplicationUser model, string senha)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.TipoUsuario != "Instrutor")
                return RedirectToAction("Index", "Home");

            var novoAluno = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NomeCompleto = model.NomeCompleto,
                Idade = model.Idade,
                Objetivo = model.Objetivo,
                TipoUsuario = "Aluno",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(novoAluno, senha);

            if (result.Succeeded)
                return RedirectToAction("Alunos");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        public async Task<IActionResult> ExcluirAluno(string id)
        {
            var aluno = await _userManager.FindByIdAsync(id);

            if (aluno != null)
                await _userManager.DeleteAsync(aluno);

            return RedirectToAction("Alunos");
        }

        public async Task<IActionResult> Treinos()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.TipoUsuario != "Instrutor")
                return RedirectToAction("Index", "Home");

            var alunos = await _context.Users
                .Where(u => u.TipoUsuario == "Aluno")
                .ToListAsync();

            ViewBag.Alunos = alunos;

            var treinos = await _context.Treinos
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
        public async Task<IActionResult> CriarTreino(CriarTreinoViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.TipoUsuario != "Instrutor")
                return RedirectToAction("Index", "Home");

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
                AlunoId = model.AlunoId
            };

            _context.Treinos.Add(treino);
            await _context.SaveChangesAsync();

            return RedirectToAction("Treinos");
        }
    }
}