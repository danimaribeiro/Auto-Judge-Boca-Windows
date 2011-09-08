using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Boca
{
    public partial class Configuracoes : Form
    {
        public Configuracoes()
        {
            InitializeComponent();
        }

        private void Configuracoes_Load(object sender, EventArgs e)
        {
            Dominio.Configuracao config = new Dominio.Configuracao();
            txtC.Text = config.CaminhoGcc;
            txtCmaimais.Text = config.CaminhoGmaismais;
            txtJava.Text = config.CaminhoJava;
            txtSubmissoes.Text = config.CaminhoSalvarSubmissoes;
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtC.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtC.Text = folderBrowserDialog1.SelectedPath;
                Dominio.Configuracao config = new Dominio.Configuracao();
                config.CaminhoGcc = txtC.Text;
                config.SalvarConfiguracao();
            }
        }

        private void btnCmaimais_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtCmaimais.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCmaimais.Text = folderBrowserDialog1.SelectedPath;
                Dominio.Configuracao config = new Dominio.Configuracao();
                config.CaminhoGmaismais = txtCmaimais.Text;
                config.SalvarConfiguracao();
            }
        }

        private void btnjava_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtJava.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtJava.Text = folderBrowserDialog1.SelectedPath;
                Dominio.Configuracao config = new Dominio.Configuracao();
                config.CaminhoJava = txtJava.Text;
                config.SalvarConfiguracao();
            }
        }

        private void btnsubmissoes_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtSubmissoes.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSubmissoes.Text = folderBrowserDialog1.SelectedPath;
                Dominio.Configuracao config = new Dominio.Configuracao();
                config.CaminhoSalvarSubmissoes = txtSubmissoes.Text;
                config.SalvarConfiguracao();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
