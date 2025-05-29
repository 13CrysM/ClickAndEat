using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClickAndEat.Model;
using ClickAndEat.Repositories;
using System.Windows.Input;
using ClickAndEat.View;

namespace ClickAndEat.ViewModel
{
    public class MenusDiariosViewModel : INotifyPropertyChanged
    {
        //private Usuario _usuarioLogueado;
        private ObservableCollection<MenuDiario> _menusDiarios;
        private MenuDiario _selectedMenuDiario;
        private MenusDiarioRepository _menuDiarioRepository;
        private Usuario usuario;
        private Usuario _usuario;


        public ObservableCollection<MenuDiario> MenusDiarios
        {
            get => _menusDiarios;
            set
            {
                _menusDiarios = value;
                OnPropertyChanged();
            }
        }
        //public Usuario UsuarioActual { get; }
        public MenuDiario SelectedMenu
        {
            get => _selectedMenuDiario;
            set
            {
                _selectedMenuDiario = value;
                OnPropertyChanged();
                // Actualiza el estado del comando Eliminar
                (EliminarMenuCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        public ICommand AgregarMenuCommand { get; }
        public ICommand EliminarMenuCommand { get; }
        /*public MenusDiariosViewModel(MenusDiarioRepository menusDiarioRepository)
        {
            _menuDiarioRepository = new MenusDiarioRepository();
            // Inicializa comandos
            //AgregarMenuCommand = new RelayCommand(AgregarMenu);
            //EliminarMenuCommand = new RelayCommand(EliminarMenu, PuedeEliminarMenu);
            CargarMenusUsuario();
            //CargarUsuarioLogueado();
            //CargarMenus(menusDiarioRepository: menusDiarioRepository);
        }
        */
        public MenusDiariosViewModel()
        {
            _menuDiarioRepository = new MenusDiarioRepository();
            CargarMenusDiarios();
        }
        public MenusDiariosViewModel(Usuario usuario)
        {
            _usuario = usuario;
            _usuario.Id = usuario.Id; // Asegúrate de que el usuario tiene un Id válido
            _menuDiarioRepository = new MenusDiarioRepository();
            //this.usuario = usuario;
            CargarMenusUsuario(_usuario.Id);
        }
        private void CargarMenusUsuario(int Id)
        {
            try
            {
                Debug.WriteLine("🟡 Iniciando carga de menus para el usuario desde la base de datos(usuario)...");

                var listaMenusUsuario = _menuDiarioRepository.ObtenerPorUsuarioId(Id);


                Debug.WriteLine($"🟢 Se obtuvieron {listaMenusUsuario.Count} menus de la BD para el usuario {Id}");
                // Log detallado de cada menu
                foreach (var menu in listaMenusUsuario)
                {
                    Debug.WriteLine($"   👤 ID: {menu.MenuId}, Usuario: {menu.UsuarioId}");
                }
                MenusDiarios = new ObservableCollection<MenuDiario>(listaMenusUsuario);
                Debug.WriteLine("🟢 Colección de menus actualizada en ViewModel");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al cargar menus usuarios: {ex.Message}");
                Debug.WriteLine($"🔴 StackTrace usuarios: {ex.StackTrace}");
            }
        }
        /*public MenusDiariosViewModel(MenusDiarioRepository menusDiarioRepository, Usuario usuario) : this(menusDiarioRepository)
        {
            this.usuario = usuario;
        }

        public MenusDiariosViewModel(Usuario usuario)
        {
            this.usuario = usuario;
        }*/

        private void CargarMenusDiarios()
        {
            try
            {
                Debug.WriteLine("🟡 Iniciando carga de menus desde la base de datos...");

                var listaMenus = _menuDiarioRepository.ObtenerTodos();

                Debug.WriteLine($"🟢 Se obtuvieron {listaMenus.Count} menus de la BD");

                // Log detallado de cada usuario
                foreach (var menu in listaMenus)
                {
                    Debug.WriteLine($"   👤 ID: {menu.MenuId}, Usuario: {menu.UsuarioId}");
                }

                MenusDiarios = new ObservableCollection<MenuDiario>(listaMenus);

                Debug.WriteLine("🟢 Colección de menus actualizada en ViewModel");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al cargar menus: {ex.Message}");
                Debug.WriteLine($"🔴 StackTrace: {ex.StackTrace}");

                // Opcional: Crear colección vacía para evitar nulls
                MenusDiarios = new ObservableCollection<MenuDiario>();
            }
        }
        /*private void CargarMenus(MenusDiarioRepository menusDiarioRepository)
        {
            var todosLosMenus = menusDiarioRepository.ObtenerTodos(); // Cambia por tu método real

            if (_usuarioLogueado != null)
            {
                if (_usuarioLogueado.Perfil == "Paciente")
                {
                    MenusDiarios = new ObservableCollection<MenuDiario>(
                        todosLosMenus.Where(m => m.UsuarioId == _usuarioLogueado.Id));
                }
                else
                {
                    MenusDiarios = new ObservableCollection<MenuDiario>(todosLosMenus);
                }
            }
            else
            {
                MenusDiarios = new ObservableCollection<MenuDiario>(); // Usuario no encontrado
            }
        }
        private void CargarUsuarioLogueado()
        {
            // Asegúrate de que esta línea accede correctamente al usuario logueado
            _usuarioLogueado = (Usuario)Application.Current.Properties["UsuarioLogueado"];
        }*/
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
