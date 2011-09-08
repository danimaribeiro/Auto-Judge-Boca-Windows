using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boca.Dominio
{
    public class Configuracao
    {
        public Configuracao()
        {
            this.CaminhoSalvarSubmissoes = Boca.Properties.Settings.Default.CaminhoSubmissao;
            this.CaminhoGcc = Boca.Properties.Settings.Default.CaminhoGcc;
            this.CaminhoGmaismais = Boca.Properties.Settings.Default.Caminhogplusplus;
            this.CaminhoJava = Boca.Properties.Settings.Default.CaminhoJava;
        }

        public string CaminhoSalvarSubmissoes { get; set; }

        public string CaminhoGcc { get; set; }

        public string CaminhoGmaismais { get; set; }

        public string CaminhoJava { get; set; }

        public void SalvarConfiguracao()
        {
            Boca.Properties.Settings.Default.CaminhoSubmissao = this.CaminhoSalvarSubmissoes;
            Boca.Properties.Settings.Default.CaminhoGcc = this.CaminhoGcc;
            Boca.Properties.Settings.Default.Caminhogplusplus = this.CaminhoGmaismais;
            Boca.Properties.Settings.Default.CaminhoJava = this.CaminhoJava;
            Boca.Properties.Settings.Default.Save();
        }
    }
}
