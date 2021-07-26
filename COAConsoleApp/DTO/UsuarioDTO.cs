using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAConsoleApp.DTO
{
    public class UsuarioDTO
    {
        [Display(Name = "Ingresar Nombre de Usuario")]
        [Required(ErrorMessage = "El Nombre de Usuario es obligatorio.")]
        public string UserName { get; set; }

        [Display(Name = "Ingresar Nombre")]
        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Display(Name = "Ingresar Email")]
        [Required(ErrorMessage = "La dirección de correo electrónico es obligatoria.")]
        [EmailAddress(ErrorMessage = "Dirección de correo electrónico inválida")]
        public string Email { get; set; }

        [Display(Name = "Ingresar Telefono")]
        public long Telefono { get; set; }

    }
}
