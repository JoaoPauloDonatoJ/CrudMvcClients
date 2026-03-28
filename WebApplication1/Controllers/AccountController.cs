using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _services;

        public AccountController(AppDbContext context, IPasswordService service)
        {
            _context = context;
            _services = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email ou senha inválidos.");
                return View(loginDto);

            }

            var isPasswordValid = await _services.VerifyPassword(loginDto.Password, user.Password);

            if (!isPasswordValid)
            {
                //ModelState.AddModelError("", isPasswordValid.Message);
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(loginDto);
            }


            TempData["Success"] = "Usuário logado com sucesso!";
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Home");




        }
    }
}
