using System;
using System.Windows;
using System.Windows.Controls;
using ClickAndEat.Model;
using System.Data.SqlClient;

namespace ClickAndEat.View
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            // Configuración inicial de los campos
            email.Text = "Email";
            password.Text = "Password";

            // Manejar eventos de foco para los campos
            email.GotFocus += RemovePlaceholderText;
            email.LostFocus += AddPlaceholderText;
            password.GotFocus += RemovePlaceholderText;
            password.LostFocus += AddPlaceholderText;
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Email" || textBox.Text == "Password")
            {
                textBox.Text = "";
                if (textBox == password)
                {
                    // Cambiar a PasswordBox si quieres mayor seguridad
                    textBox.FontWeight = FontWeights.Bold;
                }
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string userEmail = email.Text.Trim();
            string userPassword = password.Text.Trim();

            // Validaciones básicas
            if (userEmail == "Email" || string.IsNullOrWhiteSpace(userEmail))
            {
                MessageBox.Show("Por favor ingrese su email", "Campo requerido",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                email.Focus();
                return;
            }

            if (userPassword == "Password" || string.IsNullOrWhiteSpace(userPassword))
            {
                MessageBox.Show("Por favor ingrese su contraseña", "Campo requerido",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                password.Focus();
                return;
            }

            try
            {
                using (DatabaseHelper db = new DatabaseHelper())
                {
                    if (db.ValidarUsuario(userEmail, userPassword))
                    {
                        // Autenticación exitosa
                        Menu menu = new Menu();
                        menu.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Email o contraseña incorrectos", "Autenticación fallida",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        password.SelectAll();
                        password.Focus();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error de conexión a la base de datos: {ex.Message}",
                              "Error técnico", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_signup_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementarías la lógica para registro de nuevos usuarios
            MessageBox.Show("Funcionalidad de registro en construcción", "Sign Up",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void button_forgot_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementarías la recuperación de contraseña
            MessageBox.Show("Funcionalidad de recuperación en construcción", "Forgot Password",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Métodos existentes que no necesitan cambios
        private void textBox_TextChanged(object sender, TextChangedEventArgs e) { }
        private void textBox2_TextChanged(object sender, TextChangedEventArgs e) { }
        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e) { }
    }
}