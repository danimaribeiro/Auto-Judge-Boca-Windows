using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            this.Host = Boca.Properties.Settings.Default.Host;
            this.Porta = Boca.Properties.Settings.Default.Porta;
            this.Banco = Boca.Properties.Settings.Default.Banco;
            this.Usuario = Boca.Properties.Settings.Default.Usuario;
            this.Senha = Boca.Properties.Settings.Default.Senha;
        }

        public string CaminhoSalvarSubmissoes { get; set; }

        public string CaminhoGcc { get; set; }

        public string CaminhoGmaismais { get; set; }

        public string CaminhoJava { get; set; }

        public virtual string Host { get; set; }

        public virtual string Porta { get; set; }

        public virtual string Banco { get; set; }

        public virtual string Usuario { get; set; }

        public virtual string Senha { get; set; }

        public void SalvarConfiguracao()
        {
            Boca.Properties.Settings.Default.CaminhoSubmissao = this.CaminhoSalvarSubmissoes;
            Boca.Properties.Settings.Default.CaminhoGcc = this.CaminhoGcc;
            Boca.Properties.Settings.Default.Caminhogplusplus = this.CaminhoGmaismais;
            Boca.Properties.Settings.Default.CaminhoJava = this.CaminhoJava;
            Boca.Properties.Settings.Default.Host = this.Host;
            Boca.Properties.Settings.Default.Porta = this.Porta;
            Boca.Properties.Settings.Default.Banco = this.Banco;
            Boca.Properties.Settings.Default.Usuario = this.Usuario;
            Boca.Properties.Settings.Default.Senha = this.Senha;
            Boca.Properties.Settings.Default.Save();
        }

        public bool IsValido()
        {
            string gcc = System.IO.Path.Combine(this.CaminhoGcc , "gcc.exe");
            if (!System.IO.File.Exists(gcc))
            {
                MessageBox.Show("Não encontrado o Gcc para compilar. Não pode ser iniciado o auto judge.");
                return false;
            }
            string gplus = System.IO.Path.Combine(this.CaminhoGcc, "g++.exe");
            if (!System.IO.File.Exists(gplus))
            {
                MessageBox.Show("Não encontrado o G++ para compilar. Não pode ser iniciado o auto judge.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.CaminhoSalvarSubmissoes))
            {
                MessageBox.Show("Sem caminho para salvar as submissões.");
                return false;
            }
            return true;
        }
    }
}
