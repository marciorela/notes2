using Notes2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.ViewModels
{
    public class CategoriasEditVM
    {

        [Required(ErrorMessage = "Informe a descrição")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }

    public static class CategoriasEditVMExtensions
    {
        public static CategoriasEditVM ToVM(this Categoria data)
        {
            return new CategoriasEditVM
            {
                Descricao = data.Descricao
            };
        }

        public static Categoria ToModel(this CategoriasEditVM data, Categoria categoria)
        {
            categoria.Descricao = data.Descricao;

            return categoria;
        }
    }
}
