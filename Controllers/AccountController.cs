using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Identity;
using Blog.Models;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            ViewBag.Title = "Kayıt Ol";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync("user"))
                    {
                        await _userManager.AddToRoleAsync(user, "user");
                    }
                    return RedirectToAction("Login", "Account");
                }

                // Hata mesajı
                ModelState.AddModelError("", "Kullanıcı oluşturulurken bir hata oluştu.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Giriş Yap";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Hata durumlarına göre mesaj
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Hesap kilitlendi.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Giriş izni yok.");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Profil()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfilGuncelleme
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profil(ProfilGuncelleme model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Success, redirect to a confirmation page or display success message
                    return RedirectToAction("Profil", "Account");
                }

                // If something failed, add error to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
