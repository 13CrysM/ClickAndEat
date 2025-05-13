using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClickAndEat.Model;
using ClickAndEat.Repositories;
using System.Windows.Input;


namespace ClickAndEat.ViewModel
{
    public class RecordViewModel : ViewModelBase
    {
        private readonly RepositoryBase repositoryBase;

        private ObservableCollection<UserModel> _users;
        private UserModel _user;

        // Existing code...
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }

        }

        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged(nameof(Users));
                }
            }
        }
        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }

        }


        public RecordViewModel()
        {


            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
            _user = new UserModel();
            //_users = repositoryBase.Get();
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Welcome {user.Name} {user.LastName} ;)";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in.";
                //Hide child views.
            }
        }
        public ICommand AddCommand
        {
            get
            {
                return new ViewModelCommand(AddExecute, AddCanExecute);
            }
        }


        private void AddExecute(Object user)
        {
            User.Id = Guid.NewGuid().ToString();
            userRepository.Add(User);
            //Users = repositoryBase.Get();
        }
        private bool AddCanExecute(Object user)
        {
            return true;
        }
        public ICommand DeleteCommand
        {
            get
            {
                return new ViewModelCommand(DeleteExecute, DeleteCanExecute);
            }
        }

        private void DeleteExecute(Object user)
        {

            userRepository.Delete(user);    //Borra el usuario usando el id
                                            //Actualizar lista de usuarios si es necesario
                                            //Users = userRepository.Get();

        }

        private bool DeleteCanExecute(Object user)
        {
            //Verifica que el objeto user no sea nulo y tenga id válido
            return true;
        }

        public ICommand EditCommand
        {
            get
            {
                return new ViewModelCommand(EditExecute, EditCanExecute);
            }
        }
        private void EditExecute(Object user)
        {

            userRepository.Edit(User);
            //
            //

        }

        private bool EditCanExecute(Object user)
        {

            return true;
        }
    }
}
