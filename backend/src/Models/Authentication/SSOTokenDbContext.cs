using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace src.Models.Authentication
{
    // The SSOTokenDbContext class extends IdentityDbContext, which manages user authentication-related entities.
    // IdentityDbContext<IdentityUser> provides all the necessary tables for authentication,
    // such as AspNetUsers, AspNetRoles, etc.
    public class SSOTokenDbContext(DbContextOptions<SSOTokenDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        // This property defines a DbSet<SSOToken> representing the SSOTokens table in the database
        // It allows performing CRUD operations on SSOToken entities, which will be mapped to a table in the database
        public DbSet<SSOToken> SSOTokens { get; set; }
    }
}