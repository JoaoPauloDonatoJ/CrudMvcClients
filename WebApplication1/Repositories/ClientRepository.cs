using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Client>> GetAll()
        //    => await _context.Clients.ToListAsync();
        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _context.Clients
                .Include(c => c.User)
                .AsNoTracking()
                .ToListAsync();
        }
            

        public async Task<Client> GetById(int id)
        {
            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return null;
            }
                

            return client;
        }

        public async Task<Client> Add(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client;
        }

        public async Task<Client> Update(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Client> Remove(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            _context.Clients.Remove(client);
            //await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> EmailExist(string email)
        {
            return await _context.Clients
                .AnyAsync(c => c.Email == email);
        }
    }
}
