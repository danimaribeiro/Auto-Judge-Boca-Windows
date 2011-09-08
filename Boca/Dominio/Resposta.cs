using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boca.Dominio
{
    public enum Resposta
    {
        NotAnswered = 1,
        CompilationError=2,
        PresentationError=3,
        SigSegv=4,
        TimeLimit=5,
        WrongAnswer=6,
        Accept=7
    }
}
