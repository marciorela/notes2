using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRsoft.Store.Domain.Helpers;
using Notes2.Models;
using Notes2.Repositories;
using Notes2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public UsuariosController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        //[Authorize]
        //public async Task<IActionResult> Index()
        //{
        //    var usuarios = await _usuarioRepo.GetAllAsync();

        //    return View(usuarios);
        //}

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddEdit(int? id)
        {
            UsuarioEditVM usuarioEdit = null;
            if (id != null)
            {
                var usuario = await _usuarioRepo.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                usuarioEdit = usuario.ToVM();

                ViewBag.Password = false;
            }
            else
            {
                ViewBag.Password = true;
            }

            return View(usuarioEdit);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEdit(int id, UsuarioEditVM dados)
        {
            if (!ModelState.IsValid || (id == 0 && dados.Password == null))
            {
                ViewBag.Password = (id == 0);
                return View(dados);
            }

            Usuario usuario = await _usuarioRepo.GetByIdOrEmptyAsync(id);
            usuario = dados.ToModel(usuario, id);

            if (id == 0)
            {
                usuario.Senha = dados.Password.Encrypt();
            }

            _usuarioRepo.AddOrUpdate(usuario);
            await _usuarioRepo.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult NovoUsuario()
        {
            var usuario = new Usuario().ToVM();

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> NovoUsuario(UsuarioEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // VERIFICA SE O E-MAIL JÁ EXISTE NA BASE
            var checkUsuarioEmail = await _usuarioRepo.GetByEmailAsync(model.Email);
            if (checkUsuarioEmail != null)
            {
                ModelState.AddModelError("", "E-mail já cadastrado");
                return View(model);
            }

            var usuario = new Usuario();
            model.ToModel(usuario);
            usuario.Senha = model.Password.Encrypt();

            _usuarioRepo.AddOrUpdate(usuario);
            await _usuarioRepo.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
