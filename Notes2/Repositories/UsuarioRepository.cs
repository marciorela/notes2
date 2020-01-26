using Microsoft.EntityFrameworkCore;
using MRsoft.Store.Domain.Helpers;
using Notes2.Data;
using Notes2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.Repositories
{
    public class UsuarioRepository : Repository<Usuario>
    {
        public UsuarioRepository(DbNoteContext ctx) : base(ctx)
        {
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _ctx.Usuarios.ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(int? id)
        {
            return await _ctx.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> GetByIdOrEmptyAsync(int id)
        {
            if (id != 0)
            {
                return await GetByIdAsync(id);
            }
            else
            {
                return new Usuario();
            }
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _ctx.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> AuthenticateAsync(string email, string senha)
        {
            return await _ctx.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha.Encrypt());
        }

        public void AddOrUpdate(Usuario usuario)
        {
            if (usuario.Id == 0)
            {
                _ctx.Usuarios.Add(usuario);
            }
            else
            {
                _ctx.Entry(usuario).Property(p => p.DataCadastro).IsModified = false;
                _ctx.Usuarios.Update(usuario);
            }
        }
    }
}
