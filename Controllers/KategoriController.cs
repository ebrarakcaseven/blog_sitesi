using Microsoft.AspNetCore.Mvc;
using Blog.Models; // Kategori modelini içeren namespace
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class KategoriController : Controller
    {
        private readonly DataContext _db;

        public KategoriController(DataContext db)
        {
            _db = db;
        }

        // Kategori listesi
        public IActionResult Index()
        {
            var kategoriler = _db.Kategoris.ToList();
            return View(kategoriler);
        }

        // Yeni kategori ekleme GET
        public IActionResult Add()
        {
            return View();
        }

        // Yeni Kategori Ekleme (POST)
        [HttpPost]
        public IActionResult Add(Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Kategoris.Add(kategori);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Hata mesajını loglayalım
                    Console.WriteLine($"Hata: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Kategori eklenirken bir hata oluştu.");
                }
            }
            else
            {
                // ModelState hatalarını logla
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }
            return View(kategori);
        }


        // Kategori düzenleme GET
        public IActionResult Edit(int id)
        {
            var kategori = _db.Kategoris.Find(id);
            if (kategori == null) return NotFound();
            return View(kategori);
        }

        // Kategori düzenleme POST
        [HttpPost]
        public IActionResult Edit(Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                _db.Kategoris.Update(kategori);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }

        // Kategori silme GET
        public IActionResult Delete(int id)
        {
            var kategori = _db.Kategoris.Find(id);
            if (kategori == null) return NotFound();
            return View(kategori); // Silme onay sayfasını göster
        }

        // Kategori silme POST
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var kategori = _db.Kategoris.Find(id);
            if (kategori == null) return NotFound();

            _db.Kategoris.Remove(kategori);
            _db.SaveChanges();
            return RedirectToAction("Index"); // Silme işleminden sonra listeye yönlendir
        }
    }
}
