using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TreineMais.Models;

namespace TreineMais.Controllers
{
    [Authorize]
    public class RedirectController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RedirectController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Admin", "Dashboard");

            if (await _userManager.IsInRoleAsync(user, "Instrutor"))
                return RedirectToAction("Instrutor", "Dashboard");

            if (await _userManager.IsInRoleAsync(user, "Aluno"))
                return RedirectToAction("Aluno", "Dashboard");

            return RedirectToAction("Index", "Home");
        }
    }
}