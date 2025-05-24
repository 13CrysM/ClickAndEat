using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickAndEat.Model;

namespace ClickAndEat.Helpers
{
    public static class SessionManager
    {
        internal static object UsuarioActual;

        public static Usuario UsuariActual { get; set; }
    }
}
