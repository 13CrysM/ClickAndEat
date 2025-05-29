using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClickAndEat.Helpers; // para convertir SecureString a string
using ClickAndEat.Model;
using ClickAndEat.ViewModel;

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
            DataContext = new LoginViewModel();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementarías la recuperación de contraseña
            MessageBox.Show("Funcionalidad de recuperación en construcción", "Forgot Password",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }


        

        

        
        

    }
}
