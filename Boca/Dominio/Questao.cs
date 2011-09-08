using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boca.Dominio
{
    public class Questao
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string ArquivoEntrada { get; set; }

        public int IdArquivoEntrada { get; set; }

        public string ArquivoSaida { get; set; }

        public int IdArquivoSaida { get; set; }

        public int TimeLimit { get; set; }

    }
}
