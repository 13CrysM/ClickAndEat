using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClickAndEat.Model;

namespace ClickAndEat.ViewModel
{
    public class MenuViewModel : INotifyPropertyChanged
    {

        public Usuario UsuarioActual
        {
            get => _usuarioActual;
            set
            {
                _usuarioActual = value;
                /*OnPropertyChanged(nameof(UsuarioActual));
                OnPropertyChanged(nameof(EsAdministrador));
                OnPropertyChanged(nameof(EsPaciente));*/
            }
        }
        public Action AbrirUsuariosAction { get; set; }
        public Action AbrirMenusAction { get; set; }
        private Usuario _usuarioActual;
        public string TituloVentana => $"ClickAndEat - Usuario: {UsuarioActual?.Nombre} ({UsuarioActual?.Perfil})";

        // Propiedades para menú diario
        public MenuDiario MenuDiario { get; set; } = new MenuDiario { Fecha = DateTime.Today };

        // Comandos
        public ICommand GuardarMenuCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand _abrirUsuariosCommand;
        public ICommand AbrirUsuariosCommand
        {
            get { return _abrirUsuariosCommand; }
            set { _abrirUsuariosCommand = value; }
        }

        private ICommand _abrirMenusCommand;
        public ICommand AbrirMenusCommand
        {
            get
            {
                if (_abrirMenusCommand == null)
                {
                    _abrirMenusCommand = new RelayCommand(
                        _ => AbrirMenusAction?.Invoke(),
                        _ => UsuarioActual?.Perfil == "Administrador" || UsuarioActual?.Perfil == "Paciente"
                    );
                }
                return _abrirMenusCommand;
            }
        }

        public MenuViewModel(Usuario usuario)
        {


            UsuarioActual = usuario;

            Console.WriteLine("PERFIL: " + UsuarioActual?.Perfil);

            GuardarMenuCommand = new RelayCommand(_ => GuardarMenu());
            CerrarSesionCommand = new RelayCommand(_ => CerrarSesion());

            AbrirUsuariosCommand = new RelayCommand(
                _ => AbrirUsuariosAction?.Invoke(),
                _ => UsuarioActual?.Perfil == "Administrador"
            );

            /*AbrirMenusCommand = new RelayCommand(
            _ => AbrirMenusAction?.Invoke(),
            _ => UsuarioActual?.Perfil == "Administrador" ||
            UsuarioActual?.Perfil == "Paciente"
        );*/
        }
        public bool EsAdministrador => UsuarioActual?.Perfil == "Administrador";
        public bool EsPaciente => UsuarioActual?.Perfil == "Paciente";
        private void GuardarMenu()
        {
            if (!ValidarCampos())
                return;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ClickAndEat"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO MenusDiarios 
                        (Fecha, DesayunoPlatillo, DesayunoIngredientes, DesayunoDistribucion, DesayunoKcal, DesayunoComentarios,
                         ComidaPlatillo, ComidaIngredientes, ComidaDistribucion, ComidaKcal, ComidaComentarios,
                         CenaPlatillo, CenaIngredientes, CenaDistribucion, CenaKcal, CenaComentarios, UsuarioId)
                        VALUES 
                        (@Fecha, @DesayunoPlatillo, @DesayunoIngredientes, @DesayunoDistribucion, @DesayunoKcal, @DesayunoComentarios,
                         @ComidaPlatillo, @ComidaIngredientes, @ComidaDistribucion, @ComidaKcal, @ComidaComentarios,
                         @CenaPlatillo, @CenaIngredientes, @CenaDistribucion, @CenaKcal, @CenaComentarios, @UsuarioId)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Fecha", MenuDiario.Fecha);
                        cmd.Parameters.AddWithValue("@DesayunoPlatillo", MenuDiario.DesayunoPlatillo);
                        cmd.Parameters.AddWithValue("@DesayunoIngredientes", MenuDiario.DesayunoIngredientes);
                        cmd.Parameters.AddWithValue("@DesayunoDistribucion", MenuDiario.DesayunoDistribucion);
                        cmd.Parameters.AddWithValue("@DesayunoKcal", MenuDiario.DesayunoKcal);
                        cmd.Parameters.AddWithValue("@DesayunoComentarios", MenuDiario.DesayunoComentarios);

                        cmd.Parameters.AddWithValue("@ComidaPlatillo", MenuDiario.ComidaPlatillo);
                        cmd.Parameters.AddWithValue("@ComidaIngredientes", MenuDiario.ComidaIngredientes);
                        cmd.Parameters.AddWithValue("@ComidaDistribucion", MenuDiario.ComidaDistribucion);
                        cmd.Parameters.AddWithValue("@ComidaKcal", MenuDiario.ComidaKcal);
                        cmd.Parameters.AddWithValue("@ComidaComentarios", MenuDiario.ComidaComentarios);

                        cmd.Parameters.AddWithValue("@CenaPlatillo", MenuDiario.CenaPlatillo);
                        cmd.Parameters.AddWithValue("@CenaIngredientes", MenuDiario.CenaIngredientes);
                        cmd.Parameters.AddWithValue("@CenaDistribucion", MenuDiario.CenaDistribucion);
                        cmd.Parameters.AddWithValue("@CenaKcal", MenuDiario.CenaKcal);
                        cmd.Parameters.AddWithValue("@CenaComentarios", MenuDiario.CenaComentarios);

                        cmd.Parameters.AddWithValue("@UsuarioId", UsuarioActual.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Menú guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                MenuDiario = new MenuDiario { Fecha = DateTime.Today };
                OnPropertyChanged(nameof(MenuDiario));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el menú: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(MenuDiario.DesayunoPlatillo) ||
                    string.IsNullOrWhiteSpace(MenuDiario.ComidaPlatillo) ||
                    string.IsNullOrWhiteSpace(MenuDiario.CenaPlatillo))
            {
                MessageBox.Show("Todos los platillos son obligatorios", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(MenuDiario.DesayunoIngredientes) ||
                string.IsNullOrWhiteSpace(MenuDiario.ComidaIngredientes) ||
                string.IsNullOrWhiteSpace(MenuDiario.CenaIngredientes))
            {
                MessageBox.Show("Todos los ingredientes son obligatorios", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(MenuDiario.DesayunoDistribucion) ||
                string.IsNullOrWhiteSpace(MenuDiario.ComidaDistribucion) ||
                string.IsNullOrWhiteSpace(MenuDiario.CenaDistribucion))
            {
                MessageBox.Show("Las distribuciones son obligatorias", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (MenuDiario.DesayunoKcal <= 0 || MenuDiario.ComidaKcal <= 0 || MenuDiario.CenaKcal <= 0)
            {
                MessageBox.Show("Las kcal deben ser números válidos", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void CerrarSesion()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var login = new View.Login();
                login.Show();
                foreach (Window window in Application.Current.Windows)
                    if (window is View.Menu)
                        window.Close();
            });
        }
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
