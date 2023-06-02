using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraseMVC.Models
{
    public class Atividade
    {

        static MySqlConnection con = FabricaConexao.getConexao();
        static MySqlCommand query = new MySqlCommand();
        static MySqlCommand query2 = new MySqlCommand();
        static Usuario u;
        static string email = u.Email;


        private int qtdAcerto = 0;

        public int QtdAcerto { get => qtdAcerto; set => qtdAcerto = value; }
    
    public string adicionarAcerto(int qtdAcerto)
        {
            try
            {

                con.Open();
                query = new MySqlCommand("Select idUsuario from tb_usuario where email = @" + u.Email, con);

              
                    query2 = new MySqlCommand("INSERT into tb_atividade(qtdAcerto) values(@qtdAcerto) where idUsuario = @id", con);

                    query.Parameters.AddWithValue("@email", u.Email);
                    object ID = query.ExecuteScalar();
                    MySqlDataReader reader = query.ExecuteReader();


                    query2.Parameters.AddWithValue("@qtdAcerto", qtdAcerto+1);
                    query2.Parameters.AddWithValue("@id", ID);

                    query2.ExecuteNonQuery();

            }
            catch(MySqlException e)
            {
                return "Erro" + e.ToString();
            }
            finally
            {
                con.Close();
            }
            return "Sucesso";
        }
    
    }
}
