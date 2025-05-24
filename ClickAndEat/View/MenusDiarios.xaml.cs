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
using ClickAndEat.ViewModel;


namespace ClickAndEat.View
{
    /// <summary>
    /// Lógica de interacción para MenusDiarios.xaml
    /// </summary>
    public partial class MenusDiarios : Window
    {
        public MenusDiarios()
        {
            InitializeComponent();
            DataContext = new MenusDiariosViewModel(); // Asignamos el ViewModel a la vista

        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Principal principal = new Principal();
            principal.Show();
            this.Close();
        }
    }
}
