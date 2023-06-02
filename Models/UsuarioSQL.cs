using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraseMVC.Models
{
    public class UsuarioSQL
    {



        //Atributos
        private string login, email, senha, descricao;
        private byte[] bytesArquivo;
        private int id;
        private IFormFile arq;

        //Construtores

        public UsuarioSQL() {

        }

        public UsuarioSQL(IFormFile arq) {
            this.arq = arq;
        }

        public UsuarioSQL(string email, string senha) {
            this.email = email;
            this.senha = senha;
        }

        public UsuarioSQL(string usuario, string email, string senha) {
            this.login = usuario;
            this.email = email;
            this.senha = senha;
        }

        public UsuarioSQL(string email, string descricao, byte[] bytesArquivo, string login) {
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



        static SqlConnection con = FabricaConexao.getConexaoServer();
        static SqlCommand query = new SqlCommand();


        //método que cadastra o usuário

        public string cadastrarUsuario() {

            //Variável resultado que vai devolver o estado do cadastro
            string resultado = "";
            try {
                if (!usuarioCadastrado(email, login)) {
                    con.Open();

                    //inserindo os dados no banco
                    query = new SqlCommand(
                       "insert into tb_usuario(login,email,senha) values(@login, @email, @senha)", con);
                    query.Parameters.AddWithValue("@login", login.Trim().ToString());
                    query.Parameters.AddWithValue("@email", email.Trim().ToString());
                    query.Parameters.AddWithValue("@senha", senha.Trim().ToString());



                    query.ExecuteNonQuery();
                    resultado = "Cadastrado com sucesso";
                } else {

                    resultado = "Esse email ou usuário já está em uso!";

                }

            } catch (SqlException ex) {
                return ex.Message;
            } finally {
                con.Close();
            }

            return resultado;
        }


        //método para entrar na conta do usuário
        public string entrarContaUsuario() {

            //Variável resultado que vai devolver o estado do login
            string resultado = "";

            try {

                con.Open();

                //Selecionando para ver se o email existe
                query = new SqlCommand("SELECT * FROM tb_usuario where email = @email", con);




                query.Parameters.AddWithValue("@email", email.Trim().ToString());

                //leitor que irá ler cada linha no banco de dados
                SqlDataReader leitor = query.ExecuteReader();

                resultado = "Usuário não encontrado";

                while (leitor.Read()) {

                    //se a senha inserida estiver igual à do banco
                    if (leitor["senha"].ToString() == senha) {

                        resultado = "Sucesso";
                        break;
                    } else {

                        resultado = "Senha incorreta";
                        break;
                    }
                }

            } catch (SqlException f) {
                return f.ToString();
            } finally {
                con.Close(); //Fecha a conexão
            }

            return resultado;

        }

        //método que irá adicionar o avatar do usuário-
        public string adicionarAvatar(string email) {


            String tipoArquivo = arq.ContentType;

            if (tipoArquivo.Contains("image")) {
                {

                    MemoryStream s = new MemoryStream();
                    arq.CopyTo(s);
                    byte[] bytesArquivo = s.ToArray(); //Transforma o arquivo em uma sequência de bytes
                    try {
                        con.Open();

                        query = new SqlCommand("update tb_usuario set icone =@icone where email=@email", con);

                        query.Parameters.AddWithValue("@icone", bytesArquivo);
                        query.Parameters.AddWithValue("@email", email);

                        query.ExecuteNonQuery();
                        return "Sucesso";

                    } catch (SqlException f) {
                        return "Erro" + f.ToString();
                    } finally {
                        con.Close();
                    }
                }

            } else {
                return "O arquivo não é do tipo imagem";

            }


        }


        //verifica se o usuário ou email já está cadastrado
        public bool usuarioCadastrado(string email, string usuario) {

            //variável que vai retornar o resultado se for verdadeiro oufalso
            bool resultado = false;

            try {
                con.Open();

                //seleciona a tabela usuario
                query = new SqlCommand("Select * from tb_usuario", con);

                SqlDataReader reader = query.ExecuteReader();

                while (reader.Read()) {
                    //lê a coluna email e a coluna login
                    string emailCadastrado = (string)reader["email"];
                    string userCadastrado = (string)reader["login"];

                    //se os dados colocado no campo email ou senha estiverem no banco
                    //resultado = true e não deixa cadastrar
                    if (emailCadastrado == email || userCadastrado == usuario) {
                        resultado = true;
                        break;
                    } else {
                        //se não resultado = false e o cadastro pode ser efetivado
                        resultado = false;
                    }
                }
            } catch (SqlException f) {

            } finally {
                con.Close();
            }

            return resultado;

        }

        //método para adicionar a descrição
        public string addDescricao(string email) {

            try {
                con.Open();

                //atualiza a coluna descrição
                query = new SqlCommand("update tb_usuario set descricao =@descricao where email=@email", con);

                query.Parameters.AddWithValue("@descricao", descricao);
                query.Parameters.AddWithValue("@email", email);

                query.ExecuteNonQuery();
                return "Sucesso";

            } catch (SqlException f) {
                return "Erro" + f.ToString();
            } finally {
                con.Close();
            }


        }


        //metodo para pegar as infos do usuario
        static public UsuarioSQL exibirUsuario(string email) {
            UsuarioSQL conta = new UsuarioSQL();
            string imagem = "";

            //select pelo email
            try {
                con.Open();

                query = new SqlCommand("SELECT * FROM tb_usuario where email=@email", con);

                query.Parameters.AddWithValue("email", email);
                SqlDataReader reader = query.ExecuteReader();


                while (reader.Read()) {
                    string aleatorio = reader["icone"].ToString();
                    //se o ícone não estiver vazio
                    if (reader["icone"].ToString() != "") {
                        //guardar em um objeto
                        conta = new UsuarioSQL(reader["email"].ToString(), reader["descricao"].ToString(), (byte[])reader["icone"], reader["login"].ToString());
                        imagem = Convert.ToBase64String(conta.bytesArquivo);
                    } else {
                        conta = new UsuarioSQL(reader["email"].ToString(), reader["descricao"].ToString(), null, reader["login"].ToString());
                    }

                }



            } catch (SqlException f) {
            } finally {
                con.Close();
            }
            //devolve o objeto

            return conta;

        }
    }
}
