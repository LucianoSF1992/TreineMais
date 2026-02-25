using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TreineMais.Models;

namespace TreineMais.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.TipoUsuario != "Instrutor")
                return RedirectToAction("Index", "Home");

            ViewBag.Nome = user.NomeCompleto;

            return View();
        }
    }
}