using Microsoft.AspNetCore.Identity;

namespace Blog.Identity
{
    
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public ApplicationRole() : base() 
        {
        }

        public ApplicationRole(string rolename, string description) : base(rolename) 
        {
            this.Description = description;
        }
    }
}
