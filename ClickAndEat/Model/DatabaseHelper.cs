using System;
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
}
