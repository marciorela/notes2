using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes2.ViewModels
{
    public class UsuarioSignInVM
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Informe seu e-mail")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Manter conectado")]
        public bool Lembrar { get; set; }
    }
}
