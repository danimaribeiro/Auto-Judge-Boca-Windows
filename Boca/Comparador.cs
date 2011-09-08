using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boca
{
    public class Comparador
    {
        //DONE
        public int Comparar(Dominio.Questao questao, Dominio.Submissao submissao)
        {
            Dominio.Configuracao configuracao = new Dominio.Configuracao();

            string caminhoSol = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, questao.ArquivoSaida);
            string caminhoSaida = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, submissao.Id + ".out");

            System.IO.StreamReader reader = new System.IO.StreamReader(caminhoSol, System.Text.Encoding.UTF8);           

            List<string> respostaCerta = new List<string>();

            while (reader.EndOfStream ==false)
            {
                respostaCerta.Add(reader.ReadLine());
            }
            reader.Close();

            System.IO.StreamReader leitura = new System.IO.StreamReader(caminhoSaida, System.Text.Encoding.UTF8);

            List<string> respostaSubmissao = new List<string>();

            while (leitura.EndOfStream==false)
            {
                respostaSubmissao.Add(leitura.ReadLine());
            }
            leitura.Close();

            if (respostaCerta.Count != respostaSubmissao.Count)
            {
                //TODO Verificar se talvez a ultima linha não é em branco, dai retornar Presentation Error
                return -1;
            }

            for (int i = 0; i < respostaCerta.Count; i++)
            {
                string semEspacoNoFim = respostaSubmissao[i].TrimEnd(' ');
                if (respostaCerta[i] != semEspacoNoFim)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
