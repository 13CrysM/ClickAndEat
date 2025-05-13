using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ClickAndEat.Model;
using System.Collections.ObjectModel;

namespace ClickAndEat.Repositories
{
    public abstract class RepositoryBase
    {
        public readonly string _connectionString;
        public RepositoryBase()
        {
            /*_connectionString =
                "Server = ZACCMUROP\\CRYSMP; " +
                "Database=proy20240429; " +
                "Integrated Security = true";*/
            _connectionString =
                "Server = CRYSMURO\\BDCRYSM; " +
                "Database=clikEat_Cm; " +
                "Integrated Security = true";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
