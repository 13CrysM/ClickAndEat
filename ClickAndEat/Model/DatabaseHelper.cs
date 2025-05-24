/*using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; 

namespace ClickAndEat.Model
{
    public class DatabaseHelper 
    {
        private string connectionString;

        public DatabaseHelper() 
        {
            connectionString = ConfigurationManager.ConnectionStrings["ClickAndEat"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Error: la cadena de conexion no esta configurada en App.config.");
            }
        }

        //private string connectionString = "Server = DESKTOP-288DPE6\\VSGESTION; " +
        //                                "Database = ClickAndEat; " +
        //                              "Integrated Security = true";

        //private string connectionString = ConfigurationManager.ConnectionStrings["ClickAndEat"].ConnectionString;

        //private string connectionString = ConfigurationManager.ConnectionStrings["ClickAndEat"].ConnectionString;

        public void GuardarMenuCompleto(
            string desayunoPlatillo, string desayunoIngredientes, string desayunoDistribucion,
            string desayunoKcal, string desayunoComentarios,
            string comidaPlatillo, string comidaIngredientes, string comidaDistribucion,
            string comidaKcal, string comidaComentarios,
            string cenaPlatillo, string cenaIngredientes, string cenaDistribucion,
            string cenaKcal, string cenaComentarios)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}*/


using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BCrypt.Net;
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

        // Método para registrar usuarios
        public bool RegistrarUsuario(string email, string password)
        {
            // Hashear la contraseña antes de almacenarla
            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            DateTime fechaRegistro = DateTime.Now;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Usuarios (Email, Password, FechaRegistro) 
                                VALUES (@Email, @Password, @FechaRegistro)";

                SqlCommand command = new SqlCommand(query, connection);
                Debug.WriteLine($"Correo: {email}, pwd: {password}, fecha {fechaRegistro}");

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
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

        // Método para verificar credenciales de usuario
        public bool ValidarUsuario(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Password FROM Usuarios WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);

                connection.Open();
                var storedPassword = cmd.ExecuteScalar()?.ToString();

                if (storedPassword == null) return false;

                // Verificación temporal sin BCrypt (SOLO PARA DESARROLLO)
                return storedPassword == password;

                /* Versión producción (comentada):
                try {
                    return BCrypt.Net.BCrypt.Verify(password, storedPassword);
                } catch {
                    return false;
                }
                */

            }
        }
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
                string query = "SELECT Id, Email FROM Usuarios WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password); // En producción usar BCrypt

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Email = reader["Email"].ToString()
                        };
                    }
                    return null;
                }
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
        #endregion
    }
}