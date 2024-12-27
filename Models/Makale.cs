using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Makale
{
    public int Id { get; set; }
    public string Baslik { get; set; }
    public string Aciklama { get; set; }
    public DateTime YayinTarih { get; set; }
    public bool Onay { get; set; }
    public int Goruntulenme { get; set; }
    public string KullaniciAd { get; set; }
    public int KategoriId { get; set; }
    public Kategori Kategori { get; set; } // Kategori ilişkisi

    // Yorumlar koleksiyonu
    public ICollection<Yorum> Yorums { get; set; }
}

}