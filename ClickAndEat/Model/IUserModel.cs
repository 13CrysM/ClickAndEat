using System.Net;

namespace ClickAndEat.Model
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(Usuario userModel);
        void Edit(Usuario userModel);
        void Remove(int id);
        Usuario GetById(int id);
        Usuario Email(string email);
        //UserModel GetByUsername(string username);
        void Delete(object user);
        object ObtenerPorCredenciales(string email, string plainPassword);
        void GuardarMenu(MenuDiario nuevoMenu);
    }
}
