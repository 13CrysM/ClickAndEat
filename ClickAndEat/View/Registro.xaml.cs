using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security;
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
using ClickAndEat.Helpers;
using ClickAndEat.Model;

namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class Registro : Window
    {
        public Registro()
        {
            InitializeComponent();
            email.Text = "Email";
            email.GotFocus += RemovePlaceholderText;
            email.LostFocus += AddPlaceholderText;
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

        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {
            string userEmail = email.Text.Trim();
            //string userPassword = passwordControl.Password();

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
                    var usuario = db.RegistrarUsuario(userEmail, userPassword);

                    if (usuario)
                    {
                        MessageBox.Show("Usuario registrado correctamente");

                       /*SessionManager.UsuarioActual = usuario; // Desde LoginViewModel
                        Menu menuWindow = new Menu(usuario.Id);
                        menuWindow.Show();
                        this.Close();*/
                        Principal mainWindow = new Principal();
                        mainWindow.Show();
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

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
