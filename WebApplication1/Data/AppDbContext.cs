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
        public DbSet<User> Users { get; set; }
        // You can add other DbSet properties for other tables/entities here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura o relacionamento 1:1
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)           // Client tem um User
                .WithOne(u => u.Client)        // User tem um Client
                .HasForeignKey<Client>(c => c.UserId) // A chave estrangeira está em Client
                .IsRequired();                 // Torna o relacionamento obrigatório
        }
    }
}
