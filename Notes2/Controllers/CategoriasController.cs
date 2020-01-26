using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes2.Domain;
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
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly CategoriaRepository _categoriaRepo;
        private readonly IUser _user;

        public CategoriasController(CategoriaRepository categoriaRepo, IUser user)
        {
            _categoriaRepo = categoriaRepo;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaRepo.GetAllAsync();

            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int? id)
        {
            var categoria = new Categoria();
            if (id != null)
            {
                categoria = await _categoriaRepo.GetByIdAsync((int)id);
            }

            return View(categoria.ToVM());
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(int id, CategoriasEditVM dados)
        {
            if (!ModelState.IsValid)
            {
                return View(dados);
            }

            var categoria = await _categoriaRepo.FindByDescricaoAsync(dados.Descricao);
            if (categoria != null && categoria.Id != id)
            {
                ModelState.AddModelError("", "Descrição já cadastrada.");
                return View(dados);
            }
            else if (id == 0)
            {
                categoria = new Categoria();
            } else
            {
                categoria = await _categoriaRepo.GetByIdAsync(id);
            }

            dados.ToModel(categoria);

            _categoriaRepo.AddOrUpdate(categoria);
            await _categoriaRepo.SaveChangesAsync();

            return RedirectToAction("Index", "Categorias");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaRepo.GetByIdAsync(id);
            if (categoria == null)
            {
                return BadRequest();
            }

            _categoriaRepo.Delete(categoria);
            await _categoriaRepo.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
