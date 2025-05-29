using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net;
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
        public string Email { get; set; }
        public SecureString Password { get; set; }
        private readonly UsuarioRepository _repo = new UsuarioRepository();

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand LoginCommand { get; }
        public ICommand AbrirRegistroCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
            AbrirRegistroCommand = new RelayCommand(ExecuteAbrirRegistro);
        }
        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Email) && Password != null && Password.Length > 3;
        }
        private void ExecuteLogin(object parameter)
        {
            var credential = new NetworkCredential(Email, Password);
            var cre = credential.UserName.Trim();
            var pass = credential.Password.Trim();
            if (_repo.AuthenticateUser(credential))
            {
                Debug.WriteLine($"🔴 credential: {credential.UserName}, {credential.Password}");
                using (DatabaseHelper db = new DatabaseHelper())
                {
                    var usuario = db.ObtenerUsuarioPorCredenciales(cre, pass);
                    if (usuario == null)
                    {
                        MessageBox.Show("⚠ No se encontró el usuario con esas credenciales");
                    }
                    else
                    {
                        Console.WriteLine("✅ Usuario recuperado correctamente: " + usuario.Perfil);
                    }


                    Menu menuWindow = new Menu(usuario);
                    menuWindow.Show();
                    // Cerrar la ventana actual de Registro
                    Application.Current.Windows[0].Close();
                }
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas");
            }
        }
        private void ExecuteAbrirRegistro(object parameter)
        {
            Registro registroWindow = new Registro(new RegistroViewModel());
            registroWindow.Show();
            Application.Current.Windows[0].Close();
        }
        private void CloseActiveWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Window w && w.IsActive)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}