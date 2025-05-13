using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ClickAndEat.View;

namespace ClickAndEat
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*private void Application_Startup(object sender, StartupEventArgs e)
        {
            var loginView = new Login();
            loginView.Show(); //Show para mostrar la ventana
            loginView.IsVisibleChanged += (s, v) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                    loginView.Close();
                }
            };
        }*/
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainView = new Principal();
            mainView.Show(); //Show para mostrar la ventana
            mainView.IsVisibleChanged += (s, v) =>
            {
                if (mainView.IsVisible == false && mainView.IsLoaded)
                {
                    var princView = new MainWindow();
                    princView.Show();
                    mainView.Close();
                }
            };
        }
    }
}
