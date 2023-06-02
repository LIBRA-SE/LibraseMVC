using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraseMVC.Models
{
    public class Usuario
    {


        //Atributos
        private string login, email, senha, descricao;
        private byte[] bytesArquivo;
        private int id;
        private IFormFile arq;

        //Construtores

        public Usuario()
        {

        }

        public Usuario(IFormFile arq)
        {
            this.arq = arq;
        }

        public Usuario(string email, string senha)
        {
            this.email = email;
            this.senha = senha;
        }

        public Usuario(string usuario, string email, string senha)
        {
            this.login = usuario;
            this.email = email;
            this.senha = senha;
        }

        public Usuario(string email, string descricao, byte[] bytesArquivo, string login)
        {
            this.email = email;
            this.descricao = descricao;
            this.bytesArquivo = bytesArquivo;
            this.login = login;
        }

        //Getters e Setters
        public string Login { get => login; set => login = value; }
        public string Email { get => email; set => email = value; }
        public string Senha { get => senha; set => senha = value; }
        public IFormFile Arq { get => arq; set => arq = value; }
        public int Id { get => id; set => id = value; }
        public byte[] BytesArquivo { get => bytesArquivo; set => bytesArquivo = value; }
        public string Descricao { get => descricao; set => descricao = value; }



        //Pegando a conexão e criando um comando
        static MySqlConnection con = FabricaConexao.getConexao();
        static MySqlCommand query = new MySqlCommand();


        //método que cadastra o usuário

        public string cadastrarUsuario()
        {

            //Variável resultado que vai devolver o estado do cadastro
            string resultado = "";
            try
            {
                if (!usuarioCadastrado(email, login))
                {
                    con.Open();

                    //inserindo os dados no banco
                    query = new MySqlCommand(
                       "insert into tb_usuario(login,email,senha) values(@login, @email, @senha)", con);
                    query.Parameters.AddWithValue("@login", login.Trim().ToString());
                    query.Parameters.AddWithValue("@email", email.Trim().ToString());
                    query.Parameters.AddWithValue("@senha", senha.Trim().ToString());



                    query.ExecuteNonQuery();
                    resultado = "Cadastrado com sucesso";
                }
                else
                {

                    resultado = "Esse email ou usuário já está em uso!";

                }

            }
            catch (MySqlException ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }

            return resultado;
        }


        //método para entrar na conta do usuário
        public string entrarContaUsuario()
        {

            //Variável resultado que vai devolver o estado do login
            string resultado = "";

            try
            {

                con.Open();

                //Selecionando para ver se o email existe
                query = new MySqlCommand("SELECT * FROM tb_usuario where email = @email", con);




                query.Parameters.AddWithValue("@email", email.Trim().ToString());

                //leitor que irá ler cada linha no banco de dados
                MySqlDataReader leitor = query.ExecuteReader();

                resultado = "Usuário não encontrado";

                while (leitor.Read())
                {

                    //se a senha inserida estiver igual à do banco
                    if (leitor["senha"].ToString() == senha)
                    {

                        resultado = "Sucesso";
                        break;
                    }
                    else
                    {

                        resultado = "Senha incorreta";
                        break;
                    }
                }

            }
            catch (MySqlException f)
            {
                return f.ToString();
            }
            finally
            {
                con.Close(); //Fecha a conexão
            }

            return resultado;

        }

        //método que irá adicionar o avatar do usuário-
        public string adicionarAvatar(string email)
        {

            //Obtém o tipo de arquivo do objeto arq
            String tipoArquivo = arq.ContentType;

            if (tipoArquivo.Contains("image"))
            {
                //Cria um novo MemoryStream para armazenar os bytes do arquivo
                MemoryStream s = new MemoryStream();
                //Copia os dados do arquivo para o MemoryStream 's'
                arq.CopyTo(s);
                // Converte o MemoryStream 's' em um array de bytes
                byte[] bytesArquivo = s.ToArray();

                try
                {
                    con.Open();

                    query = new MySqlCommand("update tb_usuario set icone = @icone where email = @email", con);

                    query.Parameters.AddWithValue("@icone", bytesArquivo);
                    query.Parameters.AddWithValue("@email", email);

                    query.ExecuteNonQuery();

                    return "Sucesso";

                }
                catch (MySqlException f)
                {
                    return "Erro" + f.ToString();

                }
                finally
                {
                    con.Close();
                }

            }
            else
            {
                return "O arquivo não é do tipo imagem";
            }
        }


        //verifica se o usuário ou email já está cadastrado
        public bool usuarioCadastrado(string email, string usuario)
        {

            //variável que vai retornar o resultado se for verdadeiro oufalso
            bool resultado = false;

            try
            {
                con.Open();

                //seleciona a tabela usuario
                query = new MySqlCommand("Select * from tb_usuario", con);

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    //lê a coluna email e a coluna login
                    string emailCadastrado = (string)reader["email"];
                    string userCadastrado = (string)reader["login"];

                    //se os dados colocado no campo email ou senha estiverem no banco
                    //resultado = true e não deixa cadastrar
                    if (emailCadastrado == email || userCadastrado == usuario)
                    {
                        resultado = true;
                        break;
                    }
                    else
                    {
                        //se não resultado = false e o cadastro pode ser efetivado
                        resultado = false;
                    }
                }
            }
            catch (MySqlException f)
            {


            }
            finally
            {
                con.Close();
            }

            return resultado;

        }

        //método para adicionar a descrição
        public string addDescricao(string email)
        {

            try
            {
                con.Open();

                //atualiza a coluna descrição
                query = new MySqlCommand("update tb_usuario set descricao =@descricao where email=@email", con);

                query.Parameters.AddWithValue("@descricao", descricao);
                query.Parameters.AddWithValue("@email", email);

                query.ExecuteNonQuery();
                return "Sucesso";

            }
            catch (MySqlException f)
            {
                return "Erro" + f.ToString();
            }
            finally
            {
                con.Close();
            }


        }

        static public string deletarUsuario(string email)
        {
            try
            {
                con.Open(); ;

                //deletando a conta
                query = new MySqlCommand("delete from tb_usuario where email=@email", con);

                query.Parameters.AddWithValue("@email", email);

                query.ExecuteNonQuery();
                return "Conta deletada";
            }
            catch (MySqlException f)
            {
                return "Erro" + f.ToString();
            }
            finally
            {
                con.Close();
            }
        }


        //metodo para pegar as infos do usuario
        static public Usuario exibirUsuario(string email)
        {
            Usuario conta = new Usuario();
            string imagem = "";

            try
            {
                con.Open();

                query = new MySqlCommand("SELECT * FROM tb_usuario WHERE email = @email", con);

                query.Parameters.AddWithValue("email", email);

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    string aleatorio = reader["icone"].ToString();

                    //Se a coluna icone não está vazia
                    if (reader["icone"].ToString() != "")
                    {
                        // Cria um objeto do Usuario pegando os valores do banco
                        conta = new Usuario(reader["email"].ToString(), reader["descricao"].ToString(), (byte[])reader["icone"], reader["login"].ToString());
                    }
                    else
                    {
                        // Cria um novo objeto do Usuario com valores vazios para a coluna icone
                        conta = new Usuario(reader["email"].ToString(), reader["descricao"].ToString(), null, reader["login"].ToString());
                    }
                }
            }
            catch (MySqlException f)
            {
            }
            finally
            {
                con.Close();
            }

            return conta;
        }


    }
}
