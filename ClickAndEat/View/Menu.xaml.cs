using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClickAndEat.Model;
using ClickAndEat.Repositories;
using ClickAndEat.ViewModel;


namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {

        private readonly Usuario _usuario;

        public Menu(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;

            var viewModel = new MenuViewModel(usuario);
            this.DataContext = viewModel; // Asignamos el ViewModel a la vista
            usuario = viewModel.UsuarioActual; // Asegúrate de que el usuario esté inicializado correctamente
            if (usuario != null)
            {
                //this.DataContext = viewModel;
                Debug.WriteLine($"   👤 nombre: {usuario.Nombre}, id: {usuario.Id}, perfil: {usuario.Perfil}");
                this.Title = $"ClickAndEat - Usuario: {usuario.Nombre} - ({usuario.Perfil})";

                btnUsuarios.Visibility = usuario.Perfil == "Administrador" ? Visibility.Visible : Visibility.Collapsed; // Mostrar botón de Usuarios para Administrador
                btnMenus.Visibility = usuario.Perfil == "Administrador" || usuario.Perfil == "Paciente" ? Visibility.Visible : Visibility.Collapsed; // Mostrar botón de Menús para Administrador y Paciente

            }
            else
            {
                MessageBox.Show("El usuario no está definido correctamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void btnPromo_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); //Oculta la ventana Principal

            Promociones promo = new Promociones();
            promo.Closed += (s, args) =>
            {
                this.Show(); // Vuelve a mostrar la ventana original al cerrar Usuarios
            };
            promo.Show();
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); // Oculta la ventana Principal

            Usuarios listUser = new Usuarios();
            listUser.Closed += (s, args) =>
            {
                this.Show(); // Vuelve a mostrar la ventana original al cerrar Usuarios
            };
            listUser.Show();
        }

        private void btnMenus_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario == null)
            {
                MessageBox.Show("Usuario no inicializado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Hide(); // Oculta la ventana Principal
            MenusDiarios menus; // <- usuario actual
            Console.WriteLine($"   👤 btnMenus: {_usuario.Nombre}, id: {_usuario.Id}, perfil: {_usuario.Perfil}");

            if (_usuario.Perfil == "Paciente")
            {
                menus = new MenusDiarios(_usuario); // Si es paciente, pasamos el usuario

            }
            else
            {
                menus = new MenusDiarios(); // Si es administrador
            }
            menus.Closed += (s, args) =>
            {
                this.Show(); // Vuelve a mostrar la ventana original al cerrar MenusDiarios
            };
            menus.Show();
        }

    }
}
