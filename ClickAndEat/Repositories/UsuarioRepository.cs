using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using ClickAndEat.Model;
using System.ComponentModel;
using System.Net;

namespace ClickAndEat.Repositories
{
    public class UsuarioRepository : IUserRepository, IDisposable
    {
        private readonly SqlConnection _connection;
        private bool _disposed = false;

        public UsuarioRepository()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClickAndEat"].ConnectionString);
            _connection.Open();
            Debug.WriteLine("🟢 Conexión a BD establecida");
        }

        public List<Usuario> ObtenerTodos()
        {
            var usuarios = new List<Usuario>();

            try
            {
                Debug.WriteLine("🔍 Consultando usuarios en BD...");
                using (var command = new SqlCommand("SELECT Id, Email, Password, FechaRegistro FROM Usuarios", _connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"FechaRegistro: {reader["FechaRegistro"]}");
                        usuarios.Add(new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            //Nombre = reader["Nombre"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = "*****",
                            FechaRegistro = reader["FechaRegistro"] != DBNull.Value ? Convert.ToDateTime(reader["FechaRegistro"]) : DateTime.MinValue // O algún valor por defecto
                        });
                    }
                }
                Debug.WriteLine($"✅ Se obtuvieron {usuarios.Count} usuarios");
                return usuarios;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al leer usuarios: {ex.Message}");
                throw;
            }
        }
        //Add metodo
        public void Agregar(Usuario usuario) 
        {
            try
            {
                Debug.WriteLine($"🔄 Agregando usuario: {usuario.Email}");
                using (var command = new SqlCommand(
                    "INSERT INTO Usuarios (Nombre, Email, Password) VALUES (@Nombre, @Email, @Password); SELECT SCOPE_IDENTITY();",
                    _connection))
                {
                    //command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(usuario.Password ?? "tempPassword123"));

                    usuario.Id = Convert.ToInt32(command.ExecuteScalar());
                    Debug.WriteLine($"✅ Usuario agregado con ID: {usuario.Id}");
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                Debug.WriteLine("⚠️ Email ya registrado");
                throw new Exception("El email ya está registrado", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al agregar usuario: {ex.Message}");
                throw;
            }
        }
        //Delete metodo
        public bool Eliminar(int id)
        {
            try
            {
                Debug.WriteLine($"🗑️ Intentando eliminar usuario ID: {id}");
                using (var command = new SqlCommand("DELETE FROM Usuarios WHERE Id = @Id", _connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int affected = command.ExecuteNonQuery();
                    bool success = affected > 0;
                    Debug.WriteLine(success ? "✅ Usuario eliminado" : "⚠️ Usuario no encontrado");
                    return success;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al eliminar: {ex.Message}");
                throw;
            }
        }
        //GetById metodo
        public Usuario ObtenerPorId(int id)
        {
            try
            {
                Debug.WriteLine($"🔎 Buscando usuario ID: {id}");
                using (var command = new SqlCommand("SELECT Id, Nombre, Email FROM Usuarios WHERE Id = @Id", _connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                //Nombre = reader["Nombre"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString()
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al buscar usuario: {ex.Message}");
                throw;
            }
        }
        //Edit metodo
        public void Edit(Usuario userModel)
        {
            //using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                _connection.Open();
                command.Connection = _connection;
                command.CommandText = "UPDATE [User] SET Password=@Password, Name=@Name, LastName=@LastName, " +
                    "Email=@Email WHERE UserName=@UserName";
                command.Parameters.AddWithValue("@Id", userModel.Id);
                //command.Parameters.AddWithValue("@Usename", userModel.Username);
                //command.Parameters.AddWithValue("@Name", userModel.Name);
                //command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
        //AuthenticUser metodo
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            //using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                _connection.Open();
                command.Connection = _connection;
                command.CommandText = "select * from [User] where Email=@Email and [Password]=@Password";
                command.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = credential.UserName; // Use UserName as Email
                command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }
        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection?.Close();
                    _connection?.Dispose();
                    Debug.WriteLine("🔴 Conexión a BD cerrada");
                }
                _disposed = true;
            }
        }

        public void Add(Usuario userModel)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario Email(string email)
        {
            throw new NotImplementedException();
        }

        public void Delete(object user)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        public Usuario ObtenerPorCredenciales(string email, string password)
        {
            using (var command = new SqlCommand("SELECT Id, Email, Password FROM Usuarios WHERE Email = @Email", _connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read() && BCrypt.Net.BCrypt.Verify(password, reader["Password"].ToString()))
                    {
                        return new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        object IUserRepository.ObtenerPorCredenciales(string email, string plainPassword)
        {
            return ObtenerPorCredenciales(email, plainPassword);
        }

        public void GuardarMenu(MenuDiario nuevoMenu)
        {
            throw new NotImplementedException();
        }
    }
}
