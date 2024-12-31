using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Makale
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        public string Baslik { get; set; }

        public string Aciklama { get; set; }

        [DataType(DataType.Date)]
        public DateTime YayinTarih { get; set; }

        public bool Onay { get; set; }

        public int Goruntulenme { get; set; } 

        public string KullaniciAd { get; set; }

        // KategoriId zorunlu alan olarak işaretlendi
        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        public int? KategoriId { get; set; }

        // Kategori nesnesi zorunlu olmamalıdır
        public Kategori? Kategori { get; set; }

        // Yorums koleksiyonu zorunlu olmamalıdır
        public ICollection<Yorum>? Yorums { get; set; }
    }
}
