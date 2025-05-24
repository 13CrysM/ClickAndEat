using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickAndEat.Model;
using System.Windows.Input;

namespace ClickAndEat.ViewModel
{
    public class MenuViewModel: ViewModelBase
    {
        /*private readonly IUserRepository _userRepository;
        private int _usuarioId;

        // Propiedades vinculadas a la vista
        public string DesayunoPlatillo { get; set; }
        public string DesayunoIngredientes { get; set; }
        public string DesayunoDistribucion { get; set; }
        public string DesayunoKcal { get; set; }
        public string DesayunoComentarios { get; set; }
        public string ComidaPlatillo { get; set; }
        public string ComidaIngredientes { get; set; }
        public string ComidaDistribucion { get; set; }
        public string ComidaKcal { get; set; }
        public string ComidaComentarios { get; set; }
        public string CenaPlatillo { get; set; }
        public string CenaIngredientes { get; set; }
        public string CenaDistribucion { get; set; }
        public string CenaKcal { get; set; }
        public string CenaComentarios { get; set; }
        public ICommand GuardarMenuCommand { get; }
        public MenuViewModel(IUserRepository userRepository, int usuarioId)
        {
            _userRepository = userRepository;
            _usuarioId = usuarioId;

            GuardarMenuCommand = new RelayCommand(GuardarMenu, CanGuardarMenu);
        }

        private bool CanGuardarMenu() =>
            !string.IsNullOrWhiteSpace(DesayunoPlatillo) &&
            !string.IsNullOrWhiteSpace(ComidaPlatillo) &&
            !string.IsNullOrWhiteSpace(CenaPlatillo);

        private void GuardarMenu()
        {
            try
            {
                var nuevoMenu = new MenuDiario
                {
                    Fecha = DateTime.Now,
                    UsuarioId = _usuarioId,
                    DesayunoPlatillo = DesayunoPlatillo,
                    DesayunoIngrediente = DesayunoIngredientes,
                    DesayunoDistribucion = DesayunoDistribucion,
                    DesayunoKcal = int.Parse(DesayunoKcal),
                    DesayunoComentarios = DesayunoComentarios,
                    ComidaPlatillo = ComidaPlatillo,
                    ComidaIngrediente = ComidaIngredientes,
                    ComidaDistribucion = ComidaDistribucion,
                    ComidaKcal = int.Parse(ComidaKcal),
                    ComidaComentarios = ComidaComentarios,
                    CenaPlatillo = CenaPlatillo,
                    CenaIngrediente = CenaIngredientes,
                    CenaDistribucion = CenaDistribucion,
                    CenaKcal = int.Parse(CenaKcal),
                    CenaComentarios = CenaComentarios
                };

                _userRepository.GuardarMenu(nuevoMenu);
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        }*/
    }
}
