using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Identity;

namespace Blog.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string Name {get; set;}
        public string Surname {get; set;}
    }
}