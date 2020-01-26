using Microsoft.EntityFrameworkCore;
using Notes2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.Data
{
    public class DbNoteContext : DbContext
    {
        public DbNoteContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Nota> Notas { get; set; }

    }
}
