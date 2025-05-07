using ClickAndEat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ClickAndEat.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [User] VALUES(@Id, @Password, @Name, @LastName, @Email)";
                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@Usename", userModel.Username);
                command.Parameters.AddWithValue("@Name", userModel.Name);
                command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [User] where username @username and [password]=@password";
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }
        public void Edit(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Password=@Password, Name=@Name, LastName=@LastName, " +
                    "Email=@Email WHERE UserName=@UserName";
                //command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@Usename", userModel.Username);
                command.Parameters.AddWithValue("@Name", userModel.Name);
                command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
        public void Delete(object user)
        {
            if (user is UserModel userModel)
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [User] WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", userModel.Username);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                throw new ArgumentException("Invalid user object type", nameof(user));
            }
        }
    }
}
