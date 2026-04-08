using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.ViewModels;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace WebApplication1.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IClientService _services;
        private readonly IProfileRepository _profileRepository;

        public ClientsController(AppDbContext context, IClientService services, IProfileRepository profileRepository)
        {
            _context = context;
            _services = services;
            _profileRepository = profileRepository;
        }

        // GET: Clients
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var clients = await _services.GetAll();
            return View(clients);
        }

        [Authorize]
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

        [Authorize]
        // GET: Clients/Create
        public IActionResult Create()
        {
            return View(new ClientCreateDto());
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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




        }

        // GET: Clients/Edit/5
        [Authorize]
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

            var allProfiles = await _profileRepository.GetAllProfileAsync();

            // Verificamos se quem está LOGADO é Admin
            var isAdmin = User.IsInRole("Admin");


            // 3. Orquestração: Monta a ViewModel que a View espera
            var viewModel = new ClientEditViewModel
            {
                Id = client.Data.Id,
                Nome = client.Data.Nome,
                Email = client.Data.Email,
                Ativo = client.Data.Ativo,
                IsAdmin = isAdmin, // Adicione essa propriedade na sua ViewModel!

                // Mapeia todos os perfis para SelectListItems, marcando os que o cliente já tem
                AllProfiles = allProfiles.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nome,
                    // Verifica se o cliente já possui esse perfil para vir marcado (checked)
                    Selected = client.Data.Profiles.Contains(p.Nome)
                }).ToList()
            };

            return View(viewModel);


            //return View(client.Data);

        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientEditViewModel clientEditViewModel)
        {

            if (id != clientEditViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await RepopulateProfiles(clientEditViewModel); // Recarrega os perfis para a dropdown em caso de erro
                return View(clientEditViewModel);
            }

            // 2. MAPEAMENTO: Transformamos ViewModel em DTO para enviar ao Service
            var clientUpdateDto = new ClientUpdateDto
            {
                Id = clientEditViewModel.Id,
                Nome = clientEditViewModel.Nome,
                Email = clientEditViewModel.Email,
                Ativo = clientEditViewModel.Ativo,
                ProfileIds = clientEditViewModel.SelectedProfileIds // Aqui os IDs entram no fluxo de negócio
            };

            var result = await _services.Update(clientUpdateDto);

            if (!result.Success)
            {
                // Caso ocorra um erro (ex: cliente não existe ou erro de banco)
                ModelState.AddModelError("", result.Message);
                return View(clientEditViewModel);
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
        [Authorize]
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            await _services.Delete(id);
            TempData["Success"] = "Cliente deletado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para não repetir código quando precisar recarregar a tela em caso de erro
        private async Task RepopulateProfiles(ClientEditViewModel clientEditViewModel)
        {
            var allProfiles = await _profileRepository.GetAllProfileAsync();
            clientEditViewModel.AllProfiles = allProfiles.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nome,
                Selected = clientEditViewModel.SelectedProfileIds.Contains(p.Id)
            }).ToList();
        }
    }
}
