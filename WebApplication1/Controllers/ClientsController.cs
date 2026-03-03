using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace WebApplication1.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IClientService _services;

        public ClientsController(AppDbContext context, IClientService services)
        {
            _context = context;
            _services = services;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _services.GetAll();
            return View(clients);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            
                if (!ModelState.IsValid)
                {
                    return View(client);
                }

                var result = await _services.Create(client);

                if (!result.Success)
                {
                    // Caso ocorra um erro (ex: cliente não existe ou erro de banco)
                    ModelState.AddModelError("", result.Message);
                    return View(client);
                }


                    TempData["Success"] = "Cliente criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                

                //return View(client);
            
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _services.GetById(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {

            if (id != client.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(client);
            }

            var result = await _services.Update(client);

            if (!result.Success)
            {
                // Caso ocorra um erro (ex: cliente não existe ou erro de banco)
                ModelState.AddModelError("", result.Message);
                return View(client);
            }

            if (!result.HasChanges)
            {
                // Cenário: O usuário clicou em salvar sem mudar nada
                TempData["Warning"] = "Nenhuma alteração foi detectada. O registro permanece o mesmo.";
                return RedirectToAction(nameof(Index));
            }

            // Cenário: Sucesso total com alteração no banco
            TempData["Success"] = "Cliente atualizado com sucesso!";
            return RedirectToAction(nameof(Index));

            //return View(client);
        }
            
        

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
