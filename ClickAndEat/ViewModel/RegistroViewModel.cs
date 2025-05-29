using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;
using ClickAndEat.Helpers;
using ClickAndEat.Model;
using ClickAndEat.Repositories;
using ClickAndEat.View;


namespace ClickAndEat.ViewModel
{
    public class RegistroViewModel : INotifyPropertyChanged
    {
        private string _email;
        private SecureString _password;
        private string _perfil;
        private string _nombre;
        private string _direccion; // No se usa en el registro, pero se puede agregar si es necesario
        private readonly DatabaseHelper _databaseHelper;
        private readonly UsuarioRepository _repo = new UsuarioRepository();
        public event PropertyChangedEventHandler PropertyChanged;

        public string Nombre { get; set; }
        public string Email { get; set; }
        public SecureString Password { get; set; }
        public string Direccion { get; set; }
        public string Perfil { get; set; }
        public ICommand RegistrarCommand { get; }

        public RegistroViewModel()
        {
            _databaseHelper = new DatabaseHelper();
            RegistrarCommand = new RelayCommand(ExecuteRegistrar);
            Roles = new List<string> { "Administrador", "Paciente" };
        }
        private void ExecuteRegistrar(object parameter)
        {
            //var password = SecureStringHelper.ConvertToUnsecureString(SecurePassword);

            string userName = Nombre?.Trim();
            string userEmail = Email?.Trim();
            SecureString userPassword = Password;
            string userAdress = Direccion?.Trim();
            string userPerfil = Perfil?.Trim();
            string passwordPlano = SecureStringHelper.ConvertToUnsecureString(Password);
            var usuario = new Usuario
            {
                Nombre = this.Nombre,
                Email = this.Email,
                Password = passwordPlano,
                Direccion = this.Direccion,
                Perfil = this.Perfil
            };

            if (string.IsNullOrWhiteSpace(userEmail) ||
                Password == null ||
                string.IsNullOrWhiteSpace(userPerfil) ||
                string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                // Validar si el email ya está registrado
                if (_databaseHelper.UsuarioExiste(userEmail))
                {
                    MessageBox.Show("El email ya está registrado, por favor use otro.", "Error de registro",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var usuarioRegistrado = _databaseHelper.RegistrarUsuario(usuario.Nombre, usuario.Email, usuario.Password, usuario.Direccion, usuario.Perfil);

                if (usuarioRegistrado)
                {
                    MessageBox.Show("Usuario registrado correctamente");
                    // Crear instancia de la ventana de Login
                    Login loginWindow = new Login();
                    loginWindow.Show(); // Mostrar la ventana de Login

                    // Cerrar la ventana actual de Registro
                    Application.Current.Windows[0].Close();
                }
                else
                {
                    MessageBox.Show("Error al registrar el usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        public List<string> Roles { get; } = new List<string> { "Administrador", "Paciente" };

    }

}
