using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using ClickAndEat.Model;

namespace ClickAndEat.Repositories
{

    public class MenusDiarioRepository
    {
        private readonly SqlConnection _connection;
        private bool _disposed = false;

        public MenusDiarioRepository()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClickAndEat"].ConnectionString);
            _connection.Open();
            Debug.WriteLine("🟢 Conexión a BD establecida");
        }

        public List<MenuDiario> ObtenerTodos()
        {
            var menus = new List<MenuDiario>();
            try
            {
                Debug.WriteLine("🔍 Consultando menús en BD...");
                using (var command = new SqlCommand("SELECT * FROM MenusDiarios", _connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menus.Add(new MenuDiario
                        {
                            MenuId = Convert.ToInt32(reader["MenuId"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),

                            DesayunoPlatillo = reader["DesayunoPlatillo"].ToString(),
                            DesayunoIngredientes = reader["DesayunoIngredientes"].ToString(),
                            DesayunoDistribucion = reader["DesayunoDistribucion"].ToString(),
                            DesayunoKcal = Convert.ToInt32(reader["DesayunoKcal"]),
                            DesayunoComentarios = reader["DesayunoComentarios"].ToString(),

                            ComidaPlatillo = reader["ComidaPlatillo"].ToString(),
                            ComidaIngredientes = reader["ComidaIngredientes"].ToString(),
                            ComidaDistribucion = reader["ComidaDistribucion"].ToString(),
                            ComidaKcal = Convert.ToInt32(reader["ComidaKcal"]),
                            ComidaComentarios = reader["ComidaComentarios"].ToString(),

                            CenaPlatillo = reader["CenaPlatillo"].ToString(),
                            CenaIngredientes = reader["CenaIngredientes"].ToString(),
                            CenaDistribucion = reader["CenaDistribucion"].ToString(),
                            CenaKcal = Convert.ToInt32(reader["CenaKcal"]),
                            CenaComentarios = reader["CenaComentarios"].ToString(),

                            UsuarioId = Convert.ToInt32(reader["UsuarioId"])
                        });
                    }
                }
                Debug.WriteLine($"✅ Se obtuvieron {menus.Count} menús");
                return menus;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al leer menús: {ex.Message}");
                throw;
            }
        }

        public List<MenuDiario> ObtenerPorUsuarioId(int usuarioId)
        {
            var menus = new List<MenuDiario>();
            try
            {
                Debug.WriteLine($"🔍 Consultando menús del usuario {usuarioId} en BD...");
                using (var command = new SqlCommand("SELECT * FROM MenusDiarios WHERE UsuarioId = @UsuarioId", _connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menus.Add(new MenuDiario
                            {
                                MenuId = Convert.ToInt32(reader["MenuId"]),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),

                                DesayunoPlatillo = reader["DesayunoPlatillo"].ToString(),
                                DesayunoIngredientes = reader["DesayunoIngredientes"].ToString(),
                                DesayunoDistribucion = reader["DesayunoDistribucion"].ToString(),
                                DesayunoKcal = Convert.ToInt32(reader["DesayunoKcal"]),
                                DesayunoComentarios = reader["DesayunoComentarios"].ToString(),

                                ComidaPlatillo = reader["ComidaPlatillo"].ToString(),
                                ComidaIngredientes = reader["ComidaIngredientes"].ToString(),
                                ComidaDistribucion = reader["ComidaDistribucion"].ToString(),
                                ComidaKcal = Convert.ToInt32(reader["ComidaKcal"]),
                                ComidaComentarios = reader["ComidaComentarios"].ToString(),

                                CenaPlatillo = reader["CenaPlatillo"].ToString(),
                                CenaIngredientes = reader["CenaIngredientes"].ToString(),
                                CenaDistribucion = reader["CenaDistribucion"].ToString(),
                                CenaKcal = Convert.ToInt32(reader["CenaKcal"]),
                                CenaComentarios = reader["CenaComentarios"].ToString(),

                                UsuarioId = Convert.ToInt32(reader["UsuarioId"])
                            });
                        }
                    }
                }

                Debug.WriteLine($"✅ Se encontraron {menus.Count} menús para el usuario {usuarioId}");
                return menus;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔴 Error al obtener menús del usuario {usuarioId}: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection.Close();
                _connection.Dispose();
                Debug.WriteLine("🔴 Conexión a BD cerrada");
                _disposed = true;
            }
        }
        /*private List<MenuDiario> _menus = new List<MenuDiario>();

        public void Agregar(MenuDiario menu)
        {
            _menus.Add(menu);
        }

        public MenuDiario ObtenerPorId(int id)
        {
            return _menus.FirstOrDefault(m => m.MenuId == id);
        }

        public List<MenuDiario> ObtenerTodos()
        {
            return _menus;
        }*/

    }
}

