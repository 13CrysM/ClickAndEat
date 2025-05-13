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
using System.Windows.Shapes;

namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            Login inicio = new Login();
            inicio.Show();
            this.Close();
        }

        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {
            /*var recordView = new RecordsView();
            recordView.Show(); //Show para mostrar la ventana
            this.Close();*/
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            Principal menu = new Principal();
            menu.Show();
            this.Close();
        }

        private void btnPromo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
