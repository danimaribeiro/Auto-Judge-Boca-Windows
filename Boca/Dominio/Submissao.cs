using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boca.Dominio
{
    public class Submissao
    {
        public int Id { get; set; }

        public int IdProblema { get; set; }

        public string Usuario { get; set; }
        
        public Linguagem Linguagem { get; set; }

        public string Status { get; set; }

        public int IdData { get; set; }

        public  Dominio.Resposta Resposta { get; set; }

        public  int DiffTimeResposta { get; set; }
       
    }
}
