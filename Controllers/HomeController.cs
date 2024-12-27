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

    return View(makale);
}

    public ActionResult Detay(int id)
    {
        var makale=_db.Makales.Find(id);
        ViewBag.makale=makale;
        return View();
    }

}
