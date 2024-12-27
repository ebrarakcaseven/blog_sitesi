using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Yorum
{
    public int Id { get; set; }
    public string YorumMetni { get; set; }
    public DateTime YorumTarihi { get; set; }

    // Yorum bir makaleye aittir
    public int MakaleId { get; set; }
    public Makale Makale { get; set; }
}

}