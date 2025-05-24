using System.Security;
using System.Windows.Input;
using ClickAndEat.Model;
using ClickAndEat.Repositories;

namespace ClickAndEat.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;

        public string Email { get; set; } = "Email";
        public SecureString Password { get; set; }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        private bool CanLogin() => !string.IsNullOrWhiteSpace(Email) && Password?.Length > 0;

        private void Login()
        {
            string plainPassword = ConvertToUnsecureString(Password);
            var usuario = _userRepository.ObtenerPorCredenciales(Email, plainPassword);

            if (usuario != null)
            {
                // Lógica de navegación a otra vista
            }
            else
            {
                // Mostrar mensaje de error (manejado por la vista)
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            System.IntPtr unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
            try
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}