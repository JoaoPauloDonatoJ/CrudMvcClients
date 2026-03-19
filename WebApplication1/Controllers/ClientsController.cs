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
using WebApplication1.DTOs;

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

            var client = await _services.GetById(id.Value);

            if (!client.Success)
            {
                return NotFound(client.Message);
            }

            return View(client.Data);
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
        public async Task<IActionResult> Create(ClientCreateDto clientDto)
        {
            
                if (!ModelState.IsValid)
                {
                    return View(clientDto);
                }

                var result = await _services.Create(clientDto);

                if (!result.Success)
                {
                    // Caso ocorra um erro (ex: cliente não existe ou erro de banco)
                    ModelState.AddModelError("", result.Message);
                    return View(clientDto);
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
            return View(client.Data);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientUpdateDto clientUpdateDto)
        {

            if (id != clientUpdateDto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(clientUpdateDto);
            }

            var result = await _services.Update(clientUpdateDto);

            if (!result.Success)
            {
                // Caso ocorra um erro (ex: cliente não existe ou erro de banco)
                ModelState.AddModelError("", result.Message);
                return View(clientUpdateDto);
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

            
        }
            
        

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _services.GetById(id.Value);
            if (!client.Success)
            {
                return NotFound(client.Message);
            }

            return View(client.Data);
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
