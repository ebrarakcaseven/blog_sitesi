using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Identity;
using Blog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Blog.Models
{
    public class Login
    {
        [Required]
        [DisplayName("Kullanıcı Adı")]
        public string? Username{get;set;}
        [Required]
        [DisplayName("Şifre")]
        public string? Password{get;set;}
        public bool RememberMe{get;set;}
    }
}