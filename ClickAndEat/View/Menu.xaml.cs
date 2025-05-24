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
using System.Configuration;
using ClickAndEat.ViewModel;
using ClickAndEat.Repositories;


namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private int _usuarioId;

        public Menu(int usuarioId)
        {
            InitializeComponent();
            _usuarioId = usuarioId;
            this.Title += $" - Usuario: {_usuarioId}";
        }

        private void btnIngresarMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarCamposRequeridos())
                    return;

                if (_usuarioId <= 0)
                {
                    MessageBox.Show("No se ha identificado al usuario. Vuelva a iniciar sesión.",
                                    "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

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
                        cmd.Parameters.AddWithValue("@Fecha", DateTime.Today);

                        // Desayuno
                        cmd.Parameters.AddWithValue("@DesayunoPlatillo", txtDesayunoPlatillo.Text.Trim());
                        cmd.Parameters.AddWithValue("@DesayunoIngredientes", txtDesayunoIngredientes.Text.Trim());
                        cmd.Parameters.AddWithValue("@DesayunoDistribucion", txtDesayunoDistribucion.Text.Trim());
                        cmd.Parameters.AddWithValue("@DesayunoKcal", int.Parse(txtDesayunoKcal.Text.Trim()));
                        cmd.Parameters.AddWithValue("@DesayunoComentarios", txtDesayunoComentarios.Text.Trim());

                        // Comida
                        cmd.Parameters.AddWithValue("@ComidaPlatillo", txtComidaPlatillo.Text.Trim());
                        cmd.Parameters.AddWithValue("@ComidaIngredientes", txtComidaIngredientes.Text.Trim());
                        cmd.Parameters.AddWithValue("@ComidaDistribucion", txtComidaDistribucion.Text.Trim());
                        cmd.Parameters.AddWithValue("@ComidaKcal", int.Parse(txtComidaKcal.Text.Trim()));
                        cmd.Parameters.AddWithValue("@ComidaComentarios", txtComidaComentarios.Text.Trim());

                        // Cena
                        cmd.Parameters.AddWithValue("@CenaPlatillo", txtCenaPlatillo.Text.Trim());
                        cmd.Parameters.AddWithValue("@CenaIngredientes", txtCenaIngredientes.Text.Trim());
                        cmd.Parameters.AddWithValue("@CenaDistribucion", txtCenaDistribucion.Text.Trim());
                        cmd.Parameters.AddWithValue("@CenaKcal", int.Parse(txtCenaKcal.Text.Trim()));
                        cmd.Parameters.AddWithValue("@CenaComentarios", txtCenaComentarios.Text.Trim());

                        cmd.Parameters.AddWithValue("@UsuarioId", _usuarioId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Menú guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarCampos();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingresa valores numéricos válidos para las calorías.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error al guardar el menú: {ex.Message}\nCódigo: {ex.Number}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error general", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidarCamposRequeridos()
        {
            // Validar kcal numéricos
            if (!int.TryParse(txtDesayunoKcal.Text, out _))
            {
                MostrarError("Las kcal del desayuno deben ser un número válido", txtDesayunoKcal);
                return false;
            }

            if (!int.TryParse(txtComidaKcal.Text, out _))
            {
                MostrarError("Las kcal de la comida deben ser un número válido", txtComidaKcal);
                return false;
            }

            if (!int.TryParse(txtCenaKcal.Text, out _))
            {
                MostrarError("Las kcal de la cena deben ser un número válido", txtCenaKcal);
                return false;
            }

            // Validar platillos obligatorios (puedes añadir más validaciones)
            if (string.IsNullOrWhiteSpace(txtDesayunoPlatillo.Text))
            {
                MostrarError("El platillo de desayuno es requerido", txtDesayunoPlatillo);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtComidaPlatillo.Text))
            {
                MostrarError("El platillo de comida es requerido", txtComidaPlatillo);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCenaPlatillo.Text))
            {
                MostrarError("El platillo de cena es requerido", txtCenaPlatillo);
                return false;
            }

            return true;
        }

        private void MostrarError(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
        }

        private void LimpiarCampos()
        {
            // Desayuno
            txtDesayunoPlatillo.Text = "";
            txtDesayunoIngredientes.Text = "";
            txtDesayunoDistribucion.Text = "";
            txtDesayunoKcal.Text = "";
            txtDesayunoComentarios.Text = "";

            // Comida
            txtComidaPlatillo.Text = "";
            txtComidaIngredientes.Text = "";
            txtComidaDistribucion.Text = "";
            txtComidaKcal.Text = "";
            txtComidaComentarios.Text = "";

            // Cena
            txtCenaPlatillo.Text = "";
            txtCenaIngredientes.Text = "";
            txtCenaDistribucion.Text = "";
            txtCenaKcal.Text = "";
            txtCenaComentarios.Text = "";
        }
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás segura de que deseas cerrar sesión?", "Cerrar sesión", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Principal menu = new Principal();
                menu.Show();
                this.Close();
            }
            

        }
        private void btnPromo_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); //Oculta la ventana Menu

            Promociones promo = new Promociones();
            promo.Closed += (s, args) =>
            {
                this.Show(); // Vuelve a mostrar la ventana original al cerrar Usuarios
            };
            promo.Show();
        }
        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            Login inicio = new Login();
            inicio.Show();
            this.Close();
        }
        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); //Oculta la ventana Menu

            Usuarios usuarios = new Usuarios();
            usuarios.Closed += (s, args) =>
            {
                this.Show(); // Vuelve a mostrar la ventana original al cerrar Usuarios
            };
            usuarios.Show();
        }
        private void btnMenus_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); //Oculta la ventana Menu

            MenusDiarios menus = new MenusDiarios();
            menus.Closed += (s, args) =>
            {
                this.Show(); // Vuelve a mostrar la ventana original al cerrar Usuarios
            };
            menus.Show();
        }
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás segura de que deseas cerrar sesión?", "Cerrar sesión", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }
    }
}
