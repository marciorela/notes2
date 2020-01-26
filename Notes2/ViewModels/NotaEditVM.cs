using Notes2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.ViewModels
{
    public class NotaEditVM
    {
        [Required(ErrorMessage = "Informe o título da nota")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Range(minimum: 1, int.MaxValue, ErrorMessage = "Selecione uma categoria")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
    }

    public static class NotaEditVMExtensions
    {
        public static Nota ToModel(this NotaEditVM dados, Nota model)
        {
            model.Titulo = dados.Titulo;
            model.Descricao = dados.Descricao;
            model.CategoriaId = dados.CategoriaId;

            return model;
        }

        public static NotaEditVM ToVM(this Nota model)
        {
            return new NotaEditVM()
            {
                CategoriaId = model.CategoriaId,
                Titulo = model.Titulo,
                Descricao = model.Descricao
            };
        }
    }
}
