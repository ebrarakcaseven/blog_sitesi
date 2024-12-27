using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
namespace Blog.Models
{
   public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Makale> Makales { get; set; }
    public DbSet<Kategori> Kategoris { get; set; }
    public DbSet<Yorum> Yorums { get; set; }

    // Veritabanı model yapılandırmaları burada yapılabilir
}

}