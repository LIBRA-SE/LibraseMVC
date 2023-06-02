using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraseMVC.Models
{
    public class FabricaConexao
    {


        //Pegando a conexão "Default" do appsettings.json

        public static MySqlConnection getConexao() {
            return new MySqlConnection(
                Configuration().GetConnectionString("Default"));
        }

        public static SqlConnection getConexaoServer() {
            return new SqlConnection(
                Configuration().GetConnectionString("Azure"));
        }


        private static IConfigurationRoot Configuration() {
            IConfigurationBuilder builder =
                new ConfigurationBuilder().SetBasePath(
                    Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
