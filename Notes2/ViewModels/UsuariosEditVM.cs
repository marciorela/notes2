using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRsoft.Store.Domain.Helpers;
using Notes2.Models;

namespace Notes2.ViewModels
{
    public class UsuarioEditVM
    {
        [Required(ErrorMessage = "Nome deve ser informado.")]
        public string Nome { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "E-mail deve ser informado.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Informe a senha.")]
        public string Password { get; set; }

        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Redigite a senha.")]
        public string PasswordRetype { get; set; }
    }

    public static class UsuariosModelExtensions
    {
        public static UsuarioEditVM ToVM(this Usuario data)
        {
            return new UsuarioEditVM
            {
                Nome = data.Nome,
                Email = data.Email
            };
        }

        public static Usuario ToModel(this UsuarioEditVM data, Usuario usuario, int id = 0)
        {
            usuario.Id = id;
            usuario.Nome = data.Nome;
            usuario.Email = data.Email;

            return usuario;
        }


    }


}
