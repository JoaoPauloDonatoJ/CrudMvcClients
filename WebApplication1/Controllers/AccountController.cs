using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl = null)
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
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Data.Nome),
                new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString())
                // Dica: Futuramente você adicionará a Claim de Role aqui
                // new Claim(ClaimTypes.Role, result.Data.Perfil)
            };

            //Criando a Identidade do Usuário com as Claims e o Esquema de Autenticação
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //Informando ao ASP.NET Core que o usuário está autenticado e quais são suas Claims
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");

        }

       
        //Inserindo Logout Direto na Controller pois não há logíca de Logout até o momento
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            // 2. Remove o Cookie de Autenticação do navegador
            //passando o esquema de autenticação que definimos no Program.cs
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Success"] = "Você saiu do sistema com sucesso.";

            // 3. Redireciona para a Home ou para a tela de Login
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Login", "Account");
        }
    }
}
