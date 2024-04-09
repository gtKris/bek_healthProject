
using MySql.Data.MySqlClient;

namespace bek_healthProject.Models.DTO
{
    public class SecurityConfig
    {
        private static string connectionString = "server=saacapps.com;UserID=saacapps_ucatolica;Database=saacapps_training;Password=Ucat0lica";

        public static MySqlConnection GetConnection()
        {

            return new MySqlConnection(connectionString);
        }
    }
}