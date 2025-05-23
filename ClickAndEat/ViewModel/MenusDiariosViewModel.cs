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
        private ObservableCollection<MenuDiario> _menusDiarios;
        private MenuDiario _selectedMenuDiario;
        private MenusDiarioRepository _menuDiarioRepository;
        public ObservableCollection<MenuDiario> MenusDiarios
        {
            get => _menusDiarios;
            set
            {
                _menusDiarios = value;
                OnPropertyChanged();
            }
        }
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
        public MenusDiariosViewModel()
        {
            _menuDiarioRepository = new MenusDiarioRepository();
            // Inicializa comandos
            //AgregarMenuCommand = new RelayCommand(AgregarMenu);
            //EliminarMenuCommand = new RelayCommand(EliminarMenu, PuedeEliminarMenu);
            CargarMenusDiarios();
        }
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
