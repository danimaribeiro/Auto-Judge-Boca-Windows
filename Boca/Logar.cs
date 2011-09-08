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
    public partial class Logar : Form
    {
        public Logar()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Configuracoes configurar = new Configuracoes();
            configurar.ShowDialog();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;
            if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(senha))
            {
                Repositorio.RepositorioUsuario repoUsuario = new Repositorio.RepositorioUsuario();
                if (repoUsuario.AutenticarUsuario(usuario, senha))
                {                    
                    new AutoJudge().Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Usuário e senha não encontrado no banco de dados.", "Atenção");
            }
            else
                MessageBox.Show("Usuário e senha não encontrado no banco de dados.", "Atenção");
        }
    }
}
