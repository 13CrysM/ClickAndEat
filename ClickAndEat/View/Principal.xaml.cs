using System.Windows;

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
            /*Menu menu = new Menu();
            menu.Show();
            this.Close();*/
        }

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            /*Principal menu = new Principal();
            menu.Show();
            this.Close();*/
        }

        private void btnPromo_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); //Oculta la ventana Principal

            Promociones promo = new Promociones();
            promo.Closed += (s, args) =>
            {
                this.Show(); // Vuelve a mostrar la ventana original al cerrar Usuarios
            };
            promo.Show();
        }
        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.Show();
            this.Close();
        }
        private void btnMenus_Click(object sender, RoutedEventArgs e)
        {
            MenusDiarios menus = new MenusDiarios();
            menus.Show();
            this.Close();
        }
    }
}
