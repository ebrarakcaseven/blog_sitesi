using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly Models.DataContext _db;

        public AdminController(DataContext db)
        {
            _db = db;
        }
        public ActionResult Index()
        {
            ViewBag.sayi = _db.Makales.Count();
            return View();
        }
        public ActionResult YazarListesi()
        {
            var makale = _db.Makales.ToList();
            return View(makale);
        }
        public ActionResult OnayListesi()
        {
            var makale = _db.Makales.Where(i => i.Onay == true).ToList();
            return View(makale);
        }
        public ActionResult OnaysizListesi()
        {
            var makale = _db.Makales.Where(i => i.Onay == false).ToList();
            return View(makale);
        }
    }
}