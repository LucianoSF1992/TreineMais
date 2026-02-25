using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreineMais.Data;
using TreineMais.Models;

namespace TreineMais.Controllers
{
    public class ExerciciosController : Controller
    {
        private readonly AppDbContext _context;

        public ExerciciosController(AppDbContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var exercicios = await _context.Exercicios.ToListAsync();
            return View(exercicios);
        }

        // GET: Criar
        public IActionResult Create()
        {
            return View();
        }

        // POST: Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercicio exercicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(exercicio);
        }

        // GET: Editar
        public async Task<IActionResult> Edit(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
                return NotFound();

            return View(exercicio);
        }

        // POST: Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exercicio exercicio)
        {
            if (id != exercicio.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(exercicio);
        }

        // GET: Deletar
        public async Task<IActionResult> Delete(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
                return NotFound();

            return View(exercicio);
        }

        // POST: Confirmar Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio != null)
            {
                _context.Exercicios.Remove(exercicio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}