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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClickAndEat.Model;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using ClickAndEat.Helpers; // para convertir SecureString a string

namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {

            InitializeComponent();

            // Configuración inicial de los campos
            email.Text = "Email";

            //password.Text = "Password";

            // Manejar eventos de foco para los campos
            email.GotFocus += RemovePlaceholderText;
            email.LostFocus += AddPlaceholderText;
            //password.GotFocus += RemovePlaceholderText;
            //password.LostFocus += AddPlaceholderText;
        }
        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Email" || textBox.Text == "Password")
            {
                textBox.Text = "";
                //if (textBox == password)
                //{
                    // Cambiar a PasswordBox si quieres mayor seguridad
                 //   textBox.FontWeight = FontWeights.Bold;
                //}
            }
        }
        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox == email ? "Email" : "Password";
                textBox.FontWeight = FontWeights.Normal;
            }
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementarías la recuperación de contraseña
            MessageBox.Show("Funcionalidad de recuperación en construcción", "Forgot Password",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }


        

        private void button_signup_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementarías la lógica para registro de nuevos usuarios
            MessageBox.Show("Funcionalidad de registro en construcción", "Sign Up",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userEmail = email.Text.Trim();

            // Convertir SecureString a string
            string userPassword = ConvertToUnsecureString(passwordControl.Password);

            if (userEmail == "Email" || string.IsNullOrWhiteSpace(userEmail))
            {
                MessageBox.Show("Por favor ingrese su email", "Campo requerido",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                email.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(userPassword))
            {
                MessageBox.Show("Por favor ingrese su contraseña", "Campo requerido",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                passwordControl.Focus();
                return;
            }

            try
            {
                using (DatabaseHelper db = new DatabaseHelper())
                {
                    var usuario = db.ObtenerUsuarioPorCredenciales(userEmail, userPassword);

                    if (usuario != null)
                    {
                        SessionManager.UsuarioActual = usuario; // Desde LoginViewModel
                        Menu menuWindow = new Menu(usuario.Id);
                        menuWindow.Show();
                        this.Close();
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
        }
        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

    }
}
