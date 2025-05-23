﻿using System;
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


namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnIngresarMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarCamposRequeridos())
                    return;


                var dbHelper = new DatabaseHelper();
                {

                    dbHelper.GuardarMenuCompleto(
                        // Desayuno
                        txtDesayunoPlatillo.Text.Trim(),
                        txtDesayunoIngredientes.Text.Trim(),
                        txtDesayunoDistribucion.Text.Trim(),
                        txtDesayunoKcal.Text.Trim(),
                        txtDesayunoComentarios.Text.Trim(),

                        // Comida
                        txtComidaPlatillo.Text.Trim(),
                        txtComidaIngredientes.Text.Trim(),
                        txtCenaDistribucion.Text.Trim(),
                        txtComidaKcal.Text.Trim(),
                        txtComidaComentarios.Text.Trim(),

                        // Cena
                        txtCenaPlatillo.Text.Trim(),
                        txtCenaIngredientes.Text.Trim(),
                        txtCenaDistribucion.Text.Trim(),
                        txtCenaKcal.Text.Trim(),
                        txtCenaComentarios.Text.Trim()
                    );

                    MessageBox.Show("Menú guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Opcional: Limpiar los campos después de guardar
                    LimpiarCampos();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error al guardar el menú: {ex.Message}\nCodigo: {ex.Number}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidarCamposRequeridos()
        {
            if (!int.TryParse(txtDesayunoKcal.Text, out int kcalDesayuno))
            {
                MostrarError("Las kcal del desayuno deben ser un numero válido", txtDesayunoKcal);
                return false;
            }

            if (!int.TryParse(txtComidaKcal.Text, out int kcalComida))
            {
                MostrarError("Las kcal de la comida deben ser un número válido", txtComidaKcal);
                return false;
            }
            
            if (!int.TryParse(txtCenaKcal.Text, out int kcalCena))
            {
                MostrarError("Las kcal de la cena deben ser un número válido", txtCenaKcal);
                return false;
            }

// Resto de validaciones...
            return true;
        }
        /*{
            // Validar que los campos obligatorios no estén vacíos
            if (string.IsNullOrWhiteSpace(txtDesayunoPlatillo.Text))
            {
                MostrarError("Por favor ingrese el platillo del desayuno", txtDesayunoPlatillo);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDesayunoKcal.Text))
            {
                MostrarError("Por favor ingrese las kcal del desayuno", txtDesayunoKcal);
                return false;
            }

            // Agregar validaciones similares para comida y cena si son requeridas

            return true;
        }*/

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

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            Principal menu = new Principal();
            menu.Show();
            this.Close();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void btnPromo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            Login inicio = new Login();
            inicio.Show();
            this.Close();
        }

        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
