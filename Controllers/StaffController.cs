using BookLibrary.Models;
using BookLibrary.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class StaffController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly SignInManager<User> _signInManager;

        public StaffController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _usermanager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByNameAsync(logInViewModel.UserName);
                if(user == null)
                {
                    ModelState.AddModelError("", "No Account Found");
                }
                bool isfound = await _usermanager.CheckPasswordAsync(user,logInViewModel.Password);
                var isAdmin = await _usermanager.IsInRoleAsync(user, "admin");
                if(isfound && isAdmin)
                {
                    await _signInManager.SignInAsync(user, logInViewModel.RememberMe);
                    return RedirectToAction("Index", "Dashboard");
                }
                ModelState.AddModelError("", " Cannot Login as It's For Staff Only");

            }
            else
            {
                ModelState.AddModelError("", " Cannot Login as It's For Staff Only ");

            }
            return View(logInViewModel); ;
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("GetAllBook", "Book");
        }

    }
}
