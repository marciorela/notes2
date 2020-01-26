using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Notes2.Domain;
using Notes2.Models;
using Notes2.Repositories;
using Notes2.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.Controllers
{
    [Authorize]
    public class NotasController : Controller
    {
        private readonly NotaRepository _notaRepository;
        private readonly CategoriaRepository _categoriaRepository;

        public NotasController(NotaRepository notaRepository, CategoriaRepository categoriaRepository)
        {
            _notaRepository = notaRepository;
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoria, string buscar)
        {
            if (categoria == null)
            {
                categoria = Convert.ToInt32(Request.Cookies["CategoriaId"]);
                buscar = buscar ?? Request.Cookies["Buscar"];
            }
            else
            {
                Response.Cookies.Append("CategoriaId", categoria.ToString());
                Response.Cookies.Append("Buscar", buscar ?? "");
            }

            //categoria = categoria ?? 0;
            //buscar = buscar ?? "";

            ViewBag.searchString = buscar;
            ViewBag.categoriaId = categoria;
            ViewBag.Categorias = await _categoriaRepository.GetAllAsync();
            var notas = await _notaRepository.FindByTextAsync(buscar, (int)categoria);

            return View(notas);
        }

        public async Task GetCategoriasInViewBag()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            ViewBag.Categorias = categorias.Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Descricao });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var nota = await _notaRepository.FindByIdAsync(id);
            if (nota == null)
            {
                return BadRequest();
            }

            var notaVM = nota.ToVM();
            ViewBag.Categorias = await _categoriaRepository.GetAllAsync();
            ViewBag.categoriaId = notaVM.CategoriaId;
            ViewBag.NoteId = nota.Id;
            //            nota.DataCadastro.ToString(CultureInfo.CreateSpecificCulture("pt-BR")

            return View(notaVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int categoria, int? id)
        {
            Nota nota;

            if (id != null)
            {
                nota = await _notaRepository.FindByIdAsync((int)id);
            }
            else
            {
                nota = new Nota();
                nota.CategoriaId = categoria;
            }

            var notaVM = nota.ToVM();

            await GetCategoriasInViewBag();
            return View(notaVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(int? id, NotaEditVM dados)
        {
            if (!ModelState.IsValid)
            {
                await GetCategoriasInViewBag();
                return View();
            }

            var nota = new Nota();
            if (id != null)
            {
                nota = await _notaRepository.FindByIdAsync((int)id);
            }

            nota = dados.ToModel(nota);
            _notaRepository.AddOrUpdate(nota);
            await _notaRepository.SaveChangesAsync();

            TempData["Msg-Success"] = "Nota incluída com sucesso.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var nota = await _notaRepository.FindByIdAsync(id);
            if (nota == null)
            {
                return BadRequest();
            }

            _notaRepository.Delete(nota);
            await _notaRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
