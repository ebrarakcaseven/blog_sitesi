using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Identity;
using Blog.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
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
                else
                {
                    ModelState.AddModelError("hata", "Kullanıcı oluşturma hatası.");
                }
            }

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Hesap kilitlendi.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Giriş izni yok.");
                }
                else
                {
                    ModelState.AddModelError("", "Böyle bir kullanıcı bulunamadı.");
                }
            }

            // Doğrulama hatalarını görmek için
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            foreach (var error in errors)
            {
                Console.WriteLine(error); // Gerekirse logla
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Profil işlemi
        public async Task<ActionResult> Profil()
        {
            // Kullanıcı ID'sini alıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcıyı UserManager ile buluyoruz
            var user = await _userManager.FindByIdAsync(userId);

            // Profil bilgilerini ProfilGuncelleme modeline yerleştiriyoruz
            var data = new ProfilGuncelleme
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName
            };
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Profil(ProfilGuncelleme model)
        {
            // Kullanıcıyı UserManager ile buluyoruz
            var user = await _userManager.FindByIdAsync(model.Id);

            // Kullanıcı bilgilerini güncelliyoruz
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.UserName = model.UserName;

            // Güncellenmiş kullanıcıyı kaydediyoruz
            var result = await _userManager.UpdateAsync(user);
            return View("Update");
        }
        public ActionResult SifreDegistir()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SifreDegistir(SifreDegistirme model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı ID'sini almak için
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Kullanıcıyı buluyoruz
                var user = await _userManager.FindByIdAsync(userId);

                // Şifre değiştirme işlemini yapıyoruz
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return View("Update"); 
                }
                else
                {
                    // Hata durumunda error mesajı gösteriyoruz
                    ModelState.AddModelError("", "Şifre değiştirme işlemi başarısız.");
                }
            }

            return View(model); // Modeli geri döndürüyoruz
        }

    }
}
