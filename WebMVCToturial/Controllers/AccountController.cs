using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVCToturial.Data;
using WebMVCToturial.Models;
using WebMVCToturial.ViewModels;

namespace WebMVCToturial.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (user != null)
            {
                var checkPass = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if(checkPass)
                {
                    var resultLogin = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (resultLogin.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }              
            }
            TempData["Error"] = "Wrong credentials! Please try again.";
            return View(loginViewModel);
        }
    }
}
