using Microsoft.AspNetCore.Mvc;
using Blog.Models; // Kategori modelini içeren namespace
using System.Linq;
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

        [HttpPost]
public IActionResult Add(Kategori kategori)
{
    if (ModelState.IsValid)
    {
        try
        {
            _db.Kategoris.Add(kategori);
            _db.SaveChanges();

            // Debug mesajı yazdır
            Debug.WriteLine("Kategori başarıyla eklendi!");

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Hata mesajı yazdır
            Debug.WriteLine($"Hata oluştu: {ex.Message}");

            // Hata mesajını modelstate'e ekle
            ModelState.AddModelError(string.Empty, "Kategori eklenirken bir hata oluştu.");
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
                try
                {
                    _db.Kategoris.Update(kategori);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Kategori güncellenirken bir hata oluştu.");
                }
            }
            return View(kategori);
        }

        // Silme işlemi için GET aksiyonu
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var kategori = _db.Kategoris.Find(id);
            if (kategori == null) return NotFound();

            return View(kategori); // Silme onay sayfasını göster
        }

        // Silme işlemi için POST aksiyonu
        [HttpPost]
        public IActionResult DeleteConfirmed(int id) // POST aksiyonu burada id'yi alır
        {
            var kategori = _db.Kategoris.Find(id);
            if (kategori == null) return NotFound();

            _db.Kategoris.Remove(kategori);
            _db.SaveChanges();
            return RedirectToAction("Index"); // Silme işleminden sonra listeye yönlendir
        }
    }
}
