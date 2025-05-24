using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private SecureString _password;
        private readonly DatabaseHelper _databaseHelper;
        private string _errorMessage;


        public LoginViewModel()
        {
            _databaseHelper = new DatabaseHelper();
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
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
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }


        private void ExecuteLogin(object parameter)
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
                var usuarioRegistrado = _databaseHelper.ValidarUsuario(userEmail, userPassword);
                
                if (usuarioRegistrado)
                {
                    var usuario = _databaseHelper.ObtenerUsuarioPorCredenciales(userEmail, userPassword);
                    if (userEmail == "Email" || string.IsNullOrWhiteSpace(userEmail))
                    {
                        MessageBox.Show("Por favor ingrese su email", "Campo requerido",
                                      MessageBoxButton.OK, MessageBoxImage.Warning);
                        //email.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(userPassword))
                    {
                        MessageBox.Show("Por favor ingrese su contraseña", "Campo requerido",
                                      MessageBoxButton.OK, MessageBoxImage.Warning);
                        //passwordControl.Focus();
                        return;
                    }

                    try
                    {
                        //using (DatabaseHelper db = new DatabaseHelper())
                        {
                            //var usuario = db.ObtenerUsuarioPorCredenciales(userEmail, userPassword);

                            if (usuario != null)
                            {
                                SessionManager.UsuarioActual = usuario; // Desde LoginViewModel
                                Menu menuWindow = new Menu(usuario.Id);
                                menuWindow.Show();
                                //this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Email o contraseña incorrectos");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                
                    /*if (usuario != null)
                    {
                        SessionManager.UsuarioActual = usuario; // Desde LoginViewModel
                        Menu menuWindow = new Menu(usuario.Id);
                        menuWindow.Show();
                        Application.Current.Windows[0].Close();
                    }
                    else
                    {
                        MessageBox.Show("Email o contraseña incorrectos");
                    }*/
                    /*MessageBox.Show("Usuario registrado correctamente");
                    // Crear instancia de la ventana de Login
                    Menu menuWindow = new Menu(usuarioRegistrado.Id);
                    menuWindow.Show(); // Mostrar la ventana de Menu
                    
                   // Cerrar la ventana actual de Registro
                    Application.Current.Windows[0].Close();*/

                }
                else
                {
                    MessageBox.Show("Error de conexión.");
                    _errorMessage = "Error de conexión.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        public ICommand LoginCommand { get; }

        /*private readonly IUserRepository _userRepository;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email { get; set; } = "Email";
        public SecureString Password { get; set; }

        public ICommand LoginCommand { get; }

        /*public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            LoginCommand = new RelayCommand(Login, CanLogin);
        }*/

        /*private bool CanLogin() => !string.IsNullOrWhiteSpace(Email) && Password?.Length > 0;

        private void Login()
        {
            string plainPassword = ConvertToUnsecureString(Password);
            var usuario = _userRepository.ObtenerPorCredenciales(Email, plainPassword);

            if (usuario != null)
            {
                // Lógica de navegación a otra vista
            }
            else
            {
                // Mostrar mensaje de error (manejado por la vista)
            }
        }*/
        

        private bool CanExecuteLogin(object parameter) => !string.IsNullOrWhiteSpace(Email) && Password != null;

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            System.IntPtr unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
            try
            {
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