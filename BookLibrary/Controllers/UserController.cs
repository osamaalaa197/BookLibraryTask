using BookLibrary.Context;
using BookLibrary.Models;
using BookLibrary.Repository.BorrowerRepository;
using BookLibrary.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using System.Security.Claims;

namespace BookLibrary.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly IUserRepository _UserRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _context;

        public UserController(UserManager<User> userManager, IUserRepository UserRepository, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor,DataBaseContext dataBaseContext)
        {
            _usermanager = userManager;
            _UserRepository = UserRepository;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _context = dataBaseContext;

        }
        private string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInViewModel logInViewModel)
        {
 

            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByNameAsync(logInViewModel.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "No Account Found");
                }
                else
                {
                    bool found = await _usermanager.CheckPasswordAsync(user, logInViewModel.Password);
                    bool iscustomer = await _usermanager.IsInRoleAsync(user, "customer");
                    if (found && iscustomer)
                    {
                        await _signInManager.SignInAsync(user, logInViewModel.RememberMe);

                        return RedirectToAction("GetAllBook", "Book");
                    }
                }
  
            }
            return View(logInViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var nwBoorrow = new User();
                nwBoorrow.UserName = registerViewModel.UserName;
                nwBoorrow.Age = registerViewModel.Age;
                nwBoorrow.Email = registerViewModel.Email;
                nwBoorrow.PhoneNumber = registerViewModel.Phone;
                nwBoorrow.Age=registerViewModel.Age;
                var result = await _usermanager.CreateAsync(nwBoorrow, registerViewModel.Password);
                if (result.Succeeded)
                {
                    //await _usermanager.AddToRoleAsync(nwBoorrow, "customer");
                    await _roleManager.CreateAsync(new IdentityRole("customer"));

                    await _usermanager.AddToRoleAsync(nwBoorrow, "customer");
                        await _signInManager.SignInAsync(nwBoorrow, false);
                    return RedirectToAction("GetAllBook", "Book");
                    
;
                }   
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                return View();
            }
            return View(registerViewModel);

        }
        [Authorize]
        public async Task<IActionResult> signOut()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Clear();

            return RedirectToAction("GetAllBook", "Book");
        }


        [Authorize]
        [Route("userid")]
        public IActionResult ShowBookBorrow(string userid)
        {

            var borrowedBooks= _UserRepository.GetUserWihtBook(userid);
            return View(borrowedBooks);
        }
    }
}
