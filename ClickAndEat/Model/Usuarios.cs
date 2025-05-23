using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickAndEat.Model
{
    public class Usuario
    {
        public int Id { get; set; } // Identificador único

        public string Email { get; set; } // Email del usuario

        public string Password { get; set; } // Contraseña

        public DateTime FechaRegistro { get; set; } // Fecha de creación

        // Constructor opcional para inicializar propiedades
        public Usuario()
        {
            //FechaRegistro = DateTime.Now; // Inicializa la fecha de registro al crear un nuevo usuario
        }
        /*public Usuario(int id, string correo, string contraseña)
        {
            Id = id;
            Email = correo;
            Password = contraseña;
            FechaRegistro = DateTime.Now;
        }*/

        // Método para validar datos manualmente (sin DataAnnotations)
        /*public bool EsValido()
        {
            return !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password);
        }*/
    }
}
