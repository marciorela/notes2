using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Notes2.Models;
using Notes2.Repositories;
using Notes2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public AuthController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        [HttpGet]
        public IActionResult SignIn() => View();

        [HttpPost]
        public async Task<IActionResult> SignIn(string returnUrl, UsuarioSignInVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var usuario = await _usuarioRepo.AuthenticateAsync(model.Email, model.Senha);
            if (usuario == null)
            {
                TempData["Msg-Error"] = "Usuário ou senha inválido.";
//                ModelState.AddModelError("", "Usuário ou senha inválido");
                return View();
            }

            await LogIn(usuario, model.Lembrar);

            return Redirect(returnUrl ?? "/");
        }

        public async Task LogIn(Usuario usuario, bool Lembrar)
        {
            var claims = new List<Claim>()
            {
                new Claim("id", usuario.Id.ToString()),
                new Claim("nome", usuario.Nome)
            };
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                "nome", ""
            );
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal, new AuthenticationProperties()
            {
                IsPersistent = Lembrar
            });

            return;
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            Response.Cookies.Delete("CategoriaId");
            Response.Cookies.Delete("Buscar");

            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
