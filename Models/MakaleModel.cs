using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class MakaleModel{

    public int Id { get; set; }
    public string? KullaniciAd { get; set; }
    public string? Baslik { get; set; }
    public string? Aciklama { get; set; }
    public DateTime YayinTarih { get; set; }
    public bool Onay { get; set; }
    public int Goruntulenme { get; set; }
    public string? KategoriAd { get; set; }
    public ICollection<YorumModel>? Yorums { get; set; }
    }

    public class YorumModel
{
    public int Id { get; set; }
    public string YorumMetni { get; set; }
}

}