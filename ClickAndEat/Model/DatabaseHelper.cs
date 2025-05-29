using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ClickAndEat.Model
{
    public class DatabaseHelper : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private bool _disposed = false;

        public DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ClickAndEat"]?.ConnectionString;

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Error: la cadena de conexión no está configurada en App.config.");
            }
        }
        //Método para inciar sesión.
        public bool IniciarSesion(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password); // En producción usar BCrypt
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        // Método para verificar credenciales de usuario
        public void GuardarMenuCompleto(
            string desayunoPlatillo, string desayunoIngredientes, string desayunoDistribucion,
            string desayunoKcal, string desayunoComentarios,
            string comidaPlatillo, string comidaIngredientes, string comidaDistribucion,
            string comidaKcal, string comidaComentarios,
            string cenaPlatillo, string cenaIngredientes, string cenaDistribucion,
            string cenaKcal, string cenaComentarios,
            int usuarioId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO MenusDiarios (
                                DesayunoPlatillo, DesayunoIngredientes, DesayunoDistribucion, DesayunoKcal, DesayunoComentarios,
                                ComidaPlatillo, ComidaIngredientes, ComidaDistribucion, ComidaKcal, ComidaComentarios,
                                CenaPlatillo, CenaIngredientes, CenaDistribucion, CenaKcal, CenaComentarios)
                             VALUES (
                                @DesayunoPlatillo, @DesayunoIngredientes, @DesayunoDistribucion, @DesayunoKcal, @DesayunoComentarios,
                                @ComidaPlatillo, @ComidaIngredientes, @ComidaDistribucion, @ComidaKcal, @ComidaComentarios,
                                @CenaPlatillo, @CenaIngredientes, @CenaDistribucion, @CenaKcal, @CenaComentarios)";

                SqlCommand command = new SqlCommand(query, connection);

                // Parámetros Desayuno
                command.Parameters.AddWithValue("@DesayunoPlatillo", desayunoPlatillo);
                command.Parameters.AddWithValue("@DesayunoIngredientes", desayunoIngredientes);
                command.Parameters.AddWithValue("@DesayunoDistribucion", desayunoDistribucion);
                command.Parameters.AddWithValue("@DesayunoKcal", desayunoKcal);
                command.Parameters.AddWithValue("@DesayunoComentarios", desayunoComentarios);

                // Parámetros Comida
                command.Parameters.AddWithValue("@ComidaPlatillo", comidaPlatillo);
                command.Parameters.AddWithValue("@ComidaIngredientes", comidaIngredientes);
                command.Parameters.AddWithValue("@ComidaDistribucion", comidaDistribucion);
                command.Parameters.AddWithValue("@ComidaKcal", comidaKcal);
                command.Parameters.AddWithValue("@ComidaComentarios", comidaComentarios);

                // Parámetros Cena
                command.Parameters.AddWithValue("@CenaPlatillo", cenaPlatillo);
                command.Parameters.AddWithValue("@CenaIngredientes", cenaIngredientes);
                command.Parameters.AddWithValue("@CenaDistribucion", cenaDistribucion);
                command.Parameters.AddWithValue("@CenaKcal", cenaKcal);
                command.Parameters.AddWithValue("@CenaComentarios", cenaComentarios);

                command.Parameters.AddWithValue("@UsuarioId", usuarioId);

                connection.Open();
                command.ExecuteNonQuery();

            }
        }
        public Usuario ObtenerUsuarioPorCredenciales(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Usuarios WHERE Email = @Email AND Password = @Password";
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                            Perfil = reader.GetString(reader.GetOrdinal("Perfil"))
                        };
                    }
                }
            }

            return null;
        }
        public bool UsuarioExiste(string email)
        {
            string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        #region IDisposable Implementation
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
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }
        public bool RegistrarUsuario(string nombre, string email, string password, string direccion, string perfil)
        {
            // Hashear la contraseña antes de almacenarla
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            DateTime fechaRegistro = DateTime.Now;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Usuarios (Nombre, Email, Password, Direccion, Perfil, FechaRegistro) 
                                VALUES (@Nombre, @Email, @Password, @Direccion, @Perfil, @FechaRegistro)";

                SqlCommand command = new SqlCommand(query, connection);
                Debug.WriteLine($"Nombre: {nombre}, Correo: {email}, pwd: {password},Dirección: {direccion}, Perfil: {perfil}, fecha {fechaRegistro}");

                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Direccion", direccion);
                command.Parameters.AddWithValue("@Perfil", perfil);
                command.Parameters.AddWithValue("@FechaRegistro", fechaRegistro);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex) when (ex.Number == 2627) // Violación de UNIQUE KEY
                {
                    throw new Exception("Este email ya está registrado");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al registrar usuario: {ex.Message}");
                }
            }
        }
        #endregion
    }
}