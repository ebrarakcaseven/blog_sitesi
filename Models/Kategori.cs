using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kategori adı boş olamaz.")]
        public string KategoriAd { get; set; }
        public List<Makale> Makaleler { get; set; }= new List<Makale>();
    }
}