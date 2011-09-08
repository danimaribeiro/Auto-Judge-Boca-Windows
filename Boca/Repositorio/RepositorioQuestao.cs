using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using System.IO;

namespace Boca.Repositorio
{
    public class RepositorioQuestao
    {

        public List<Dominio.Questao> Questoes(int idContest)
        {
            string sql = "select problemnumber, problemname, probleminputfilename, problemsolfilename, problemtimelimit, probleminputfile, problemsolfile from problemtable where contestnumber = @numero and problemfullname !~ '(DEL)'";
            List<Dominio.Questao> questoes = new List<Dominio.Questao>();

            NpgsqlConnection conexao = new NpgsqlConnection("Server=187.45.196.224;Database=bubblesort9;User ID=bubblesort9;Password=BSboca;");

            try
            {
                conexao.Open();
                NpgsqlCommand comando = new NpgsqlCommand(sql, conexao);
                comando.Parameters.Add("numero", NpgsqlTypes.NpgsqlDbType.Integer).Value = idContest;
                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Dominio.Questao questao = new Dominio.Questao();
                    questao.Id = reader.GetInt32(0);
                    questao.Nome = reader.GetString(1);
                    questao.ArquivoEntrada = reader.GetString(2);
                    questao.ArquivoSaida = reader.GetString(3);
                    questao.TimeLimit = int.Parse(reader.GetString(4));
                    questao.IdArquivoEntrada = (int)reader.GetInt64(5);
                    questao.IdArquivoSaida = (int)reader.GetInt64(6);

                    questoes.Add(questao);
                }
                reader.Close();
                return questoes;
            }
            finally
            {
                conexao.Close();
            }
        }

        public void DownloadQuestao(Dominio.Questao questao)
        {
            Dominio.Configuracao configuracao = new Dominio.Configuracao();

            string caminhoEntrada = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, questao.ArquivoEntrada);
            string caminhoSaida = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, questao.ArquivoSaida);

            NpgsqlConnection conexao = new NpgsqlConnection("Server=187.45.196.224;Database=bubblesort9;User ID=bubblesort9;Password=BSboca;");

            NpgsqlTransaction transacao = null;
            try
            {
                conexao.Open();

                transacao = conexao.BeginTransaction();

                LargeObjectManager lbm = new LargeObjectManager(conexao);

                LargeObject lo = lbm.Open(questao.IdArquivoEntrada, LargeObjectManager.READ);
               

                FileStream fsout = File.OpenWrite(caminhoEntrada);

                byte[] buf = new byte[lo.Size()];

                buf = lo.Read(lo.Size());

                fsout.Write(buf, 0, (int)lo.Size());
                fsout.Flush();
                fsout.Close();
                lo.Close();

                lo = lbm.Open(questao.IdArquivoSaida, LargeObjectManager.READ);
               

                fsout = File.OpenWrite(caminhoSaida);

                byte[] buffer = new byte[lo.Size()];

                buffer = lo.Read(lo.Size());

                fsout.Write(buffer, 0, (int)lo.Size());
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
