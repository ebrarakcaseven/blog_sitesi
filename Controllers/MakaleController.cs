using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers
{
    public class MakaleController : Controller
    {
        private readonly DataContext _db;

        public MakaleController(DataContext db)
        {
            _db = db;
        }

        // Makale listesi
        public IActionResult Index()
        {
            var username = User.Identity.Name;
            var makale = _db.Makales.Where(i => i.KullaniciAd == username).Select(i => new MakaleModel()
            {
                Id = i.Id,
                Baslik = i.Baslik,
                KullaniciAd = i.KullaniciAd,
                YayinTarih = i.YayinTarih,
                Onay = i.Onay,
                Goruntulenme = i.Goruntulenme,
                Aciklama = i.Aciklama.Length > 20 ? i.Aciklama.Substring(0, 20) + "[...]" : i.Aciklama,
                Yorums = i.Yorums.Select(y => new YorumModel
                {
                    // Yorumla ilgili gerekli alanlar burada
                    Id = y.Id,
                    YorumMetni = y.YorumMetni
                }).ToList() // YorumModel'e dönüştürme
            })
       .ToList();

            return View(makale);
        }

        // Yeni makale ekleme GET
        public IActionResult Create()
        {
            ViewData["Kategoriler"] = new SelectList(_db.Kategoris, "Id", "KategoriAd");
            return View();
        }


        // Yeni makale ekleme POST
        [HttpPost]
        public IActionResult Create(Makale makale)
        {
            if (ModelState.IsValid)
            {
                makale.KullaniciAd = User.Identity.Name;
                _db.Makales.Add(makale);
                _db.SaveChanges();
                if (this.User.Identity.Name == "admin")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Onay");
                }

            }
            ViewData["Kategoriler"] = new SelectList(_db.Kategoris, "Id", "KategoriAd");
            return View(makale);
        }

        // Makale düzenleme GET
        public IActionResult Edit(int id)
        {
            var makale = _db.Makales.Find(id);
            if (makale == null)
            {
                return NotFound();
            }

            // Kategorileri ViewData'ya ekleyin
            ViewData["Kategoriler"] = new SelectList(_db.Kategoris, "Id", "KategoriAd", makale.KategoriId);
            return View(makale);
        }
        // Makale düzenleme POST
        [HttpPost]
        public IActionResult Edit(Makale makale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingMakale = _db.Makales.Find(makale.Id);
                    if (existingMakale == null)
                    {
                        Console.WriteLine("Hata: Düzenlenecek makale bulunamadı.");
                        return NotFound();
                    }

                    // Mevcut kaydı güncelle
                    existingMakale.Baslik = makale.Baslik;
                    existingMakale.Aciklama = makale.Aciklama;
                    existingMakale.YayinTarih = makale.YayinTarih;
                    existingMakale.Onay = makale.Onay;
                    existingMakale.Goruntulenme = makale.Goruntulenme;
                    existingMakale.KategoriId = makale.KategoriId;
                    existingMakale.KullaniciAd = makale.KullaniciAd;

                    _db.SaveChanges(); // Veritabanını güncelle
                    Console.WriteLine("Başarılı: Makale başarıyla güncellendi.");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                }
            }
            else
            {
                // ModelState geçerli değilse hata mesajlarını logla
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Hata: {error.ErrorMessage}");
                }
            }

            // Kategorileri yeniden doldurun
            ViewData["Kategoriler"] = new SelectList(_db.Kategoris, "Id", "KategoriAd", makale.KategoriId);
            return View(makale);
        }


        // Makale detayları
        public IActionResult Details(int id)
        {
            var makale = _db.Makales.Include(m => m.Kategori).FirstOrDefault(m => m.Id == id);
            if (makale == null)
            {
                return NotFound(); // Makale bulunamadığında NotFound dönecektir
            }
            TempData["Message"] = "Makale bulundu!";
            return View(makale); // Makale bulunduysa, detayları görüntüleyen sayfayı döndürür
        }
        // Makale Silme GET
        public IActionResult Delete(int id)
        {
            var makale = _db.Makales.FirstOrDefault(m => m.Id == id);
            if (makale == null)
            {
                return NotFound();
            }

            return View(makale);  // Makale detayını göster
        }

        // Makale Silme POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var makale = _db.Makales.FirstOrDefault(m => m.Id == id);
            if (makale == null)
            {
                return NotFound();
            }

            _db.Makales.Remove(makale);
            _db.SaveChanges();

            return RedirectToAction("Index");  // Başka bir sayfaya yönlendir
        }
    }
}
