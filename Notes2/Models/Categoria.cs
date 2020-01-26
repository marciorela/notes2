using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.Models
{
    public class Categoria : Entity
    {
        [Required]
        public string Descricao { get; set; }

        public virtual List<Nota> Notas { get; set; }

        [Required]
        public int UsuarioId { get; set; }
    }
}
