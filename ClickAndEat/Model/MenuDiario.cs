using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickAndEat.Model
{
    public class MenuDiario
    {
        public int MenuId { get; set; } // Identificador único

        public DateTime Fecha { get; set; } // Fecha de creación automática
        public String DesayunoPlatillo { get; set; }
        public String DesayunoIngrediente { get; set; }
        public String DesayunoDistribucion { get; set; }
        public int DesayunoKcal { get; set; }
        public String DesayunoComentarios { get; set; }
        public String ComidaPlatillo { get; set; }
        public String ComidaIngrediente { get; set; }
        public String ComidaDistribucion { get; set; }
        public int ComidaKcal { get; set; }
        public String ComidaComentarios { get; set; }
        public String CenaPlatillo { get; set; }
        public String CenaIngrediente { get; set; }
        public String CenaDistribucion { get; set; }
        public int CenaKcal { get; set; }
        public String CenaComentarios { get; set; }
        public int UsuarioId { get; set; } // Identificador del usuario que creó el menú

        // Constructor para inicializar valores
        public MenuDiario()
        {
            /*MenuId = id;
            DesayunoPlatillo = desayuno;
            ComidaPlatillo = comida;
            CenaPlatillo = cena;*/
            //Fecha = DateTime.Now; // Se asigna automáticamente al crear un menú

        }

        // Método para validar si los datos son correctos
        /*public bool EsValido()
        {
            return UsuarioId > 0;
        }*/
    }

}

