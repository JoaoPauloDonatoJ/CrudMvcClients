using WebApplication1.Models;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByEmail(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User?> Remove(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            _context.Users.Remove(user);
            //await _context.SaveChangesAsync();
            return user;
        }
    }
}
