using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Identity
{
    public class IdentityDataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
    {
    }
}


}
