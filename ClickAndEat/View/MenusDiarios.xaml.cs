using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClickAndEat.Model;
using ClickAndEat.Repositories;
using ClickAndEat.ViewModel;


namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para MenusDiarios.xaml
    /// </summary>
    public partial class MenusDiarios : Window
    {
        private Usuario _usuario;
        private MenusDiarioRepository _menuDiarioRepository;
        public MenusDiarios()
        {
            InitializeComponent();
            DataContext = new MenusDiariosViewModel(); // Asignamos el ViewModel a la vista

        }
        public MenusDiarios(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            Console.WriteLine($"   👤 MenusDiarios: {_usuario.Nombre}, id: {_usuario.Id}, perfil: {_usuario.Perfil}");
            //_menuDiarioRepository = new MenusDiarioRepository();

            //var listaMenus = _menuDiarioRepository.ObtenerPorUsuarioId(_usuario.Id); // <--- aquí truena si _usuario es null
            Console.WriteLine($"   👤 user menusdiarios a ViewModel: {_usuario.Id}");
            DataContext = new MenusDiariosViewModel(_usuario);

        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            /*Principal principal = new Principal();
            principal.Show();*/
            this.Close();
        }
    }
}
