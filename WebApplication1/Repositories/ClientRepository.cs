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

        public async Task<IEnumerable<Client>> GetAll()
            => await _context.Clients.ToListAsync();

        public async Task<Client> GetById(int id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                throw new KeyNotFoundException("Cliente não localizado");

            return client;
        }

        public async Task<Client> Add(Client client)
        {
           
            //client.Ativo = true;

            //var emailExist = await _context.Clients
            //    .AnyAsync(c => c.Email == client.Email);

            //if (emailExist)
            //{
            //    throw new InvalidOperationException("Email já cadastrado");
            //}


            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client;
        }

        public async Task<Client> Update(Client client)
        {

            //if (client.id == null)
            //{
            //    return NotFound();
            //}

            //var client = await _context.Clients.FindAsync(id);
            //if (client == null)
            //{
            //    return NotFound();
            //}
            //return View(client);

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> Remove(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            //if (client == null)
            //{
            //    throw new KeyNotFoundException("Cliente não localizado");
            //    //return NotFound();
            //}
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> EmailExist(string email)
        {
            return await _context.Clients
                .AnyAsync(c => c.Email == email);
        }
    }
}
