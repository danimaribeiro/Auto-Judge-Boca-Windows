using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Security.Cryptography;

namespace Boca.Repositorio
{
    public class RepositorioUsuario
    {

        public bool AutenticarUsuario(string usuario, string senha)
        {
            string sql = "select usernumber from usertable where username = @usuario and userpassword = @senha";
            List<Dominio.Submissao> submissoes = new List<Dominio.Submissao>();

            NpgsqlConnection conexao = new NpgsqlConnection("Server=187.45.196.224;Database=bubblesort9;User ID=bubblesort9;Password=BSboca;");

            try
            {
                conexao.Open();
                NpgsqlCommand comando = new NpgsqlCommand(sql, conexao);
                comando.Parameters.Add("usuario", NpgsqlTypes.NpgsqlDbType.Varchar).Value = usuario;
                comando.Parameters.Add("senha", NpgsqlTypes.NpgsqlDbType.Varchar).Value = GetMD5Hash(senha);
                var reader = comando.ExecuteReader();
                bool resposta;
                if (reader.HasRows)
                {
                    resposta = true;
                }
                else
                    resposta = false;
                reader.Close();
                return resposta;
            }
            finally
            {
                conexao.Close();
            }
        }

        public string GetMD5Hash(string input)
        {

            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

    }
}
