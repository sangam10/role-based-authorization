using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Tasklist.Controllers
{
    /*[Authorize(Roles ="Admin,Manager")]*/
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var RolesList = _roleManager.Roles.ToList();
            return View(RolesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> create(IdentityRole role)
        {
            if (!_roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult()) { 
                await _roleManager.CreateAsync(new IdentityRole(role.Name));
            }
            return RedirectToAction("Index");
        }
    }
}
