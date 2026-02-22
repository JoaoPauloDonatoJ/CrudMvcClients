using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        // For modern ASP.NET Core MVC (using dependency injection)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // For older ASP.NET MVC 5 (EF6)
        // public SchoolContext() : base("name=SchoolContext")
        // {
        // }

        public DbSet<Client> Clients { get; set; }
        // You can add other DbSet properties for other tables/entities here
    }
}
