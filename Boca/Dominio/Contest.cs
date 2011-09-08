using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boca.Dominio
{
    public class Contest
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime DataInicio { get; set; }

        public int Tempo { get; set; }
    }
}
