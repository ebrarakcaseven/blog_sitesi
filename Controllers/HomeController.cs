using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using blogsitesi.Models;
using Blog.Models;
using System;
using System.Data.Common;
namespace blogsitesi.Controllers;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly DataContext _db;

    public HomeController(DataContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        var makale = _db.Makales
            .Where(i => i.Onay == true)
            .Select(i => new MakaleModel()
            {
                Id = i.Id,
                Baslik = i.Baslik,
                KullaniciAd = i.KullaniciAd,
                YayinTarih = i.YayinTarih,
                Onay = i.Onay,
                Goruntulenme = i.Goruntulenme,
                Aciklama = i.Aciklama.Length > 60 ? i.Aciklama.Substring(0, 60) + "[...]" : i.Aciklama,
                Yorums = i.Yorums.Select(y => new YorumModel
                {
                    // Yorumla ilgili gerekli alanlar burada
                    Id = y.Id,
                    YorumMetni = y.YorumMetni
                }).ToList() // YorumModel'e dönüştürme
            })
            .ToList();
        var kategoriler = _db.Kategoris.ToList();

        ViewBag.kategoriler = kategoriler;
        return View(makale);
    }
    public ActionResult MakaleListesi(int? id)
    {
        var makale = _db.Makales.Where(i => i.Onay == true).AsQueryable();
        if (id != null)
        {
            makale = makale.Where(İ => İ.KategoriId == id);
        }
        // Kategoriler listesini doldur
        ViewBag.Kategoriler = _db.Kategoris.ToList();
        return View(makale);
    }
    public ActionResult Ara(string deger)
    {
        var ara = _db.Makales.Where(i => i.Onay == true && i.Aciklama.Contains(deger)).ToList();
        ViewBag.Kategoriler = _db.Kategoris.ToList();
        return View(ara);
    }
    public ActionResult Detay(int id)
    {
        var makale = _db.Makales.Find(id);  // Makaleyi bul
        if (makale == null)
        {
            return NotFound();  // Eğer makale bulunamazsa 404 döndür
        }

        // Makale ve onun yorumları
        var yorumlar = _db.Yorums.Where(y => y.MakaleId == id).ToList();  // Bu makaleye ait yorumları al

        // Veriyi View'a gönder
        ViewBag.makale = makale;
        ViewBag.Yorumlar = yorumlar;
        ViewBag.Kategoriler = _db.Kategoris.ToList();  // Kategorileri ekleyelim

        // Görüntülenme sayısını artır
        makale.Goruntulenme += 1;
        _db.SaveChanges();

        // Boş bir Yorum modeli gönderiyoruz ki kullanıcı yeni bir yorum yazabilsin
        return View("Detay", new Yorum() { MakaleId = id });
    }



    public ActionResult YorumGonder(Yorum yorum)
    {
        if (string.IsNullOrWhiteSpace(yorum.YorumMetni))
        {
            // Yorum metni boş ise kullanıcıyı uyar
            TempData["errorMessage"] = "Yorum metni boş olamaz!";
            return RedirectToAction("Detay", new { id = yorum.MakaleId });
        }

        // Kullanıcı bilgilerini ekle
        yorum.KullaniciId = User.Identity.Name;
        yorum.YorumTarihi = DateTime.Now;

        // Yorum veritabanına ekle
        _db.Yorums.Add(yorum);
        _db.SaveChanges();

        // Yorum eklendikten sonra makale detayına yönlendir
        return RedirectToAction("Detay", "Home", new { id = yorum.MakaleId });
    }



}
