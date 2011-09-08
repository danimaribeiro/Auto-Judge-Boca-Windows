using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace Boca.Repositorio
{
    public class RepositorioContest
    {

        public List<Dominio.Contest> Contests()
        {
            string sql = "select contestnumber, contestname, conteststartdate, contestduration from contesttable where contestactive = true";
            List<Dominio.Contest> contests = new List<Dominio.Contest>();

            NpgsqlConnection conexao = new NpgsqlConnection("Server=187.45.196.224;Database=bubblesort9;User ID=bubblesort9;Password=BSboca;");

            try
            {
                conexao.Open();
                NpgsqlCommand comando = new NpgsqlCommand(sql, conexao);
                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Dominio.Contest contest = new Dominio.Contest();
                    contest.Id = reader.GetInt32(0);
                    contest.Nome = reader.GetString(1);
                    int inicio = reader.GetInt32(2);
                    int duracao = reader.GetInt32(3);
                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    contest.DataInicio = epoch.AddSeconds(inicio).ToLocalTime();
                    contest.Tempo = duracao;
                    contests.Add(contest);
                }
                reader.Close();
                return contests;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
