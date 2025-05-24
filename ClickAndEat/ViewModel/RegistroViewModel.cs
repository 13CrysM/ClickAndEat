using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;
using ClickAndEat.Model;
using ClickAndEat.Helpers;
using ClickAndEat.View;


namespace ClickAndEat.ViewModel
{
    public class RegistroViewModel : INotifyPropertyChanged
    {
        private string _email;
        private SecureString _password;
        private readonly DatabaseHelper _databaseHelper;

        public RegistroViewModel()
        {
            _databaseHelper = new DatabaseHelper();
            RegistrarCommand = new RelayCommand(ExecuteRegistrar, CanExecuteRegistrar);
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegistrarCommand { get; }

        private void ExecuteRegistrar(object parameter)
        {
            string userEmail = Email?.Trim();
            string userPassword = ConvertToUnsecureString(Password);

            if (string.IsNullOrWhiteSpace(userEmail) || userEmail == "Email")
            {
                MessageBox.Show("Por favor ingrese su email", "Campo requerido",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(userPassword))
            {
                MessageBox.Show("Por favor ingrese su contraseña", "Campo requerido",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var usuarioRegistrado = _databaseHelper.RegistrarUsuario(userEmail, userPassword);

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
                    MessageBox.Show("Email o contraseña incorrectos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool CanExecuteRegistrar(object parameter) => !string.IsNullOrWhiteSpace(Email) && Password != null;

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null) return string.Empty;
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
