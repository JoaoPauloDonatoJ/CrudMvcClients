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
        private readonly IAuthService _authService;

        public AccountController(AppDbContext context, IPasswordService service, IAuthService authService)
        {
            _context = context;
            _services = service;
            _authService = authService;
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

            var result = await _authService.Authenticate(loginDto);

            if (!result.Success)
            {
                
                ModelState.AddModelError("", result.Message);
                return View(loginDto);
            }

            //Autenticação bem-sucedida, armazenar informações do usuário na sessão
            HttpContext.Session.SetString("UsuarioNome", result.Data.Nome);
            HttpContext.Session.SetInt32("UserId", result.Data.Id);

            TempData["Success"] = "Usuário logado com sucesso!";
            return RedirectToAction("Index", "Home");

        }

        //Inserindo Logout Direto na Controller pois não há logíca de Logout até o momento
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Success"] = "Você saiu do sistema com sucesso.";

            // 3. Redireciona para a Home ou para a tela de Login
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Login", "Account");
        }
    }
}
