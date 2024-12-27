﻿// <auto-generated />
using System;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace blogsitesi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Blog.Models.Kategori", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("KategoriAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Kategoris");
                });

            modelBuilder.Entity("Blog.Models.Makale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Baslik")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Goruntulenme")
                        .HasColumnType("int");

                    b.Property<int>("KategoriId")
                        .HasColumnType("int");

                    b.Property<string>("KullaniciAd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Onay")
                        .HasColumnType("bit");

                    b.Property<DateTime>("YayinTarih")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("KategoriId");

                    b.ToTable("Makales");
                });

            modelBuilder.Entity("Blog.Models.Yorum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MakaleId")
                        .HasColumnType("int");

                    b.Property<string>("YorumMetni")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("YorumTarihi")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MakaleId");

                    b.ToTable("Yorums");
                });

            modelBuilder.Entity("Blog.Models.Makale", b =>
                {
                    b.HasOne("Blog.Models.Kategori", "Kategori")
                        .WithMany("Makaleler")
                        .HasForeignKey("KategoriId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kategori");
                });

            modelBuilder.Entity("Blog.Models.Yorum", b =>
                {
                    b.HasOne("Blog.Models.Makale", "Makale")
                        .WithMany("Yorums")
                        .HasForeignKey("MakaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Makale");
                });

            modelBuilder.Entity("Blog.Models.Kategori", b =>
                {
                    b.Navigation("Makaleler");
                });

            modelBuilder.Entity("Blog.Models.Makale", b =>
                {
                    b.Navigation("Yorums");
                });
#pragma warning restore 612, 618
        }
    }
}
