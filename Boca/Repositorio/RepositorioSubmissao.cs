using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using System.IO;
using System.Windows.Forms;

namespace Boca.Repositorio
{
    public class RepositorioSubmissao
    {

        public List<Dominio.Submissao> SubmissoesNovas()
        {
            string sql = "select runnumber, userfullname, runlangnumber, runstatus, rundata, runproblem from runtable inner join usertable on runtable.usernumber = usertable.usernumber where runstatus = 'openrun'";
            List<Dominio.Submissao> submissoes = new List<Dominio.Submissao>();

            NpgsqlConnection conexao = new NpgsqlConnection("Server=187.45.196.224;Database=bubblesort9;User ID=bubblesort9;Password=BSboca;");

            try
            {
                conexao.Open();
                NpgsqlCommand comando = new NpgsqlCommand(sql, conexao);
                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Dominio.Submissao submissao = new Dominio.Submissao();
                    submissao.Id = reader.GetInt32(0);
                    submissao.Usuario = reader.GetString(1);
                    submissao.Linguagem = (Dominio.Linguagem)reader.GetInt32(2);
                    submissao.Status = reader.GetString(3);
                    submissao.IdData = (int)reader.GetInt64(4);
                    submissao.IdProblema = reader.GetInt32(5);
                    submissao.Resposta = Dominio.Resposta.NotAnswered;
                    submissoes.Add(submissao);
                }
                reader.Close();
                return submissoes;
            }
            finally
            {
                conexao.Close();
            }
        }


        public void Atualizar(Dominio.Submissao submissao)
        {
            string sql = "update runtable set runanswer=@resposta, runstatus=@status, runjudge=@juiz, runjudgesite=1, rundatediffans=@diff where runnumber = @numero";

            NpgsqlConnection conexao = new NpgsqlConnection("Server=187.45.196.224;Database=bubblesort9;User ID=bubblesort9;Password=BSboca;");

            try
            {
                conexao.Open();
                NpgsqlCommand comando = new NpgsqlCommand(sql, conexao);
                comando.Parameters.Add("resposta", NpgsqlDbType.Integer).Value = submissao.Resposta;
                comando.Parameters.Add("status", NpgsqlDbType.Varchar).Value = submissao.Status;
                comando.Parameters.Add("juiz", NpgsqlDbType.Integer).Value = 2;
                comando.Parameters.Add("numero", NpgsqlDbType.Integer).Value = submissao.Id;
                comando.Parameters.Add("diff", NpgsqlDbType.Integer).Value = submissao.DiffTimeResposta;
                comando.ExecuteNonQuery();
            }
            finally
            {
                conexao.Close();
            }
        }

        public void DownloadArquivos(Dominio.Submissao submissao)
        {
            Dominio.Configuracao configuracao = new Dominio.Configuracao();
            string caminho = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, submissao.Id + ".c");

            NpgsqlConnection conexao = new NpgsqlConnection("Server=187.45.196.224;Database=bubblesort9;User ID=bubblesort9;Password=BSboca;");
            NpgsqlTransaction transacao = null;
            try
            {
                conexao.Open();
                transacao = conexao.BeginTransaction();

                LargeObjectManager lbm = new LargeObjectManager(conexao);

                LargeObject lo = lbm.Open(submissao.IdData, LargeObjectManager.READWRITE);
                                
                FileStream fsout = File.OpenWrite(caminho);

                byte[] buf = new byte[lo.Size()];

                buf = lo.Read(lo.Size());

                fsout.Write(buf, 0, (int)lo.Size());
                fsout.Flush();
                fsout.Close();
                lo.Close();
                transacao.Commit();
            }
            catch
            {
                if (transacao != null)
                    transacao.Rollback();
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }


    }
}
