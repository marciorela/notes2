using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Notes2.Repositories;
using Notes2.ViewModels;
using Microsoft.EntityFrameworkCore;
using MRsoft.Store.Domain.Helpers;
using Notes2.Data;
using Notes2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Notes2.Domain;

namespace Notes2.Repositories
{
    public class NotaRepository : Repository<Nota>
    {
        private readonly IUser _user;

        public NotaRepository(DbNoteContext ctx, IUser user) : base(ctx)
        {
            _user = user;
        }

        public async Task<Nota> FindByIdAsync(int id)
        {
            return await _ctx.Notas
                .Include(c => c.Categoria)
                .FirstOrDefaultAsync(n => n.Categoria.UsuarioId == _user.Id && n.Id == id);
        }

        public async Task<IEnumerable<Nota>> FindByTextAsync(string text, int categoriaId = 0)
        {
            var notas = new List<Nota>();

            if (text == null)
            {
                notas = await _ctx.Notas
                    .Include(c => c.Categoria)
                    .Where(n => n.Categoria.UsuarioId == _user.Id && (categoriaId != 0 && n.Categoria.Id == categoriaId || categoriaId == 0))
                    .ToListAsync();
            }
            else
            {
                notas = await _ctx.Notas
                    .Where(n => n.Categoria.UsuarioId == _user.Id && (n.Titulo.ToLower().RemoveAccents().Contains(text.ToLower().RemoveAccents()) || n.Descricao.ToLower().RemoveAccents(). Contains(text.ToLower().RemoveAccents())))
                    .Where(n => (categoriaId != 0 && n.Categoria.Id == categoriaId || categoriaId == 0))
                    .ToListAsync();
            }

            return notas;
        }

        public async void AddOrUpdate(Nota nota)
        {
            if (nota.Id == 0)
            {
                if (nota.Descricao == null)
                {
                    nota.Descricao = "";
                }
                await _ctx.Notas.AddAsync(nota);
            }
            else
            {
                _ctx.Entry(nota).Property(p => p.DataCadastro).IsModified = false;
                _ctx.Notas.Update(nota);
            }
        }

        public void Delete(Nota nota)
        {
            _ctx.Notas.Remove(nota);
        }
    }
}
