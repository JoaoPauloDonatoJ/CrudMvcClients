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
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
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

            modelBuilder.Entity<UserProfile>()
            .HasKey(up => new { up.UserId, up.ProfileId });

            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User) // UserProfile tem um User
                .WithMany(u => u.UserProfiles) // User tem muitos UserProfiles
                .HasForeignKey(up => up.UserId); //A chave estrangeira está em UserProfile (Tabela de junção)

            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.Profile) // UserProfile tem um Profile
                .WithMany(p => p.UserProfiles) // Profile tem muitos UserProfiles
                .HasForeignKey(up => up.ProfileId); //A chave estrangeira está em UserProfile (Tabela de junção)

            modelBuilder.Entity<Profile>().HasData(
                new Profile { Id = 1, Nome = "Admin" },
                new Profile { Id = 2, Nome = "Operator" },
                new Profile { Id = 3, Nome = "Client" } // Adicionado para os usuários comuns
            );
        }
    }
}
