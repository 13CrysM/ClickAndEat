using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ClickAndEat.Model;
using ClickAndEat.Repositories;

namespace ClickAndEat.ViewModel
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Usuario> _usuarios;
        private Usuario _selectedUsuario;
        private UsuarioRepository _usuarioRepository;

        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set
            {
                _usuarios = value;
                OnPropertyChanged();
            }
        }

        public Usuario SelectedUsuario
        {
            get => _selectedUsuario;
            set
            {
                _selectedUsuario = value;
                OnPropertyChanged();
                // Actualiza el estado del comando Eliminar
                (EliminarUsuarioCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AgregarUsuarioCommand { get; }
        public ICommand EliminarUsuarioCommand { get; }

        public UsuariosViewModel()
        {
            _usuarioRepository = new UsuarioRepository();

            // Inicializa comandos
            //AgregarUsuarioCommand = new RelayCommand(AgregarUsuario);
            //EliminarUsuarioCommand = new RelayCommand(EliminarUsuario, PuedeEliminarUsuario);

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                Debug.WriteLine("🟡 Iniciando carga de usuarios desde la base de datos...");

                var listaUsuarios = _usuarioRepository.ObtenerTodos();

                Debug.WriteLine($"🟢 Se obtuvieron {listaUsuarios.Count} usuarios de la BD");

                // Log detallado de cada usuario
                foreach (var usuario in listaUsuarios)
                {
                    Debug.WriteLine($"   👤 ID: {usuario.Id}, Email: {usuario.Email}");
                }

                Usuarios = new ObservableCollection<Usuario>(listaUsuarios);

                Debug.WriteLine("🟢 Colección de usuarios actualizada en ViewModel");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 ERROR al cargar usuarios: {ex.Message}");
                Debug.WriteLine($"🔴 StackTrace: {ex.StackTrace}");

                // Opcional: Crear colección vacía para evitar nulls
                Usuarios = new ObservableCollection<Usuario>();
            }
        }

        /*private void AgregarUsuario()
        {
            var nuevoUsuario = new Usuario {Id = 99,  Email = "nuevo@email.com", Password = "Nuevo Usuario", };
            _usuarioRepository.Agregar(nuevoUsuario);
            Usuarios.Add(nuevoUsuario);
            SelectedUsuario = nuevoUsuario;
        }*/

        /*private void EliminarUsuario()
        {
            if (SelectedUsuario != null)
            {
                _usuarioRepository.Eliminar(SelectedUsuario.Id);
                Usuarios.Remove(SelectedUsuario);
            }
        }*/

        private bool PuedeEliminarUsuario() => SelectedUsuario != null;

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
