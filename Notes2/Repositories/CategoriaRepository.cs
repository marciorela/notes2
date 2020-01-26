using Microsoft.EntityFrameworkCore;
using Notes2.Data;
using Notes2.Domain;
using Notes2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes2.Repositories
{
    public class CategoriaRepository : Repository<Categoria>
    {
        private readonly IUser _user;

        public CategoriaRepository(DbNoteContext ctx, IUser user) : base(ctx)
        {
            _user = user;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            var categorias = await 
                _ctx.Categorias
                .Include(n => n.Notas)
                .Where(c => c.UsuarioId == _user.Id)
                .OrderBy(c => c.Descricao)
                .ToListAsync();
            return categorias;
        }

        public async Task<Categoria> GetByIdAsync(int id)
        {
            return await _ctx.Categorias.FirstOrDefaultAsync(c => c.UsuarioId == _user.Id && c.Id == id);
        }

        public async Task<Categoria> FindByDescricaoAsync(string descricao)
        {
            return await _ctx.Categorias.FirstOrDefaultAsync(c => c.Descricao.ToUpper() == descricao.ToUpper() && c.UsuarioId == _user.Id);
        }

        public void AddOrUpdate(Categoria categoria)
        {
            if (categoria.Id == 0)
            {
                categoria.UsuarioId = _user.Id;
                _ctx.Categorias.Add(categoria);
            }
            else
            {
                _ctx.Entry(categoria).Property(p => p.DataCadastro).IsModified = false;
                _ctx.Categorias.Update(categoria);
            }
            _ctx.SaveChanges();
        }

        public void Delete(Categoria categoria)
        {
            _ctx.Remove(categoria);
        }

    }
}
