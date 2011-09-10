using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using System.IO;

namespace Boca
{
    public partial class AutoJudge : Form
    {
        private Repositorio.RepositorioContest _ContestRepositorio;
        private Repositorio.RepositorioQuestao _QuestaoRepositorio;
        private Repositorio.RepositorioSubmissao _RepositorioSubmissao;

        private List<Dominio.Submissao> _SubmissoesNaoJulgadas;
        private List<Dominio.Submissao> _SubmissoesJulgadas;
        private List<Dominio.Questao> _Questoes;

        public AutoJudge()
        {
            InitializeComponent();
            _ContestRepositorio = new Repositorio.RepositorioContest();
            _QuestaoRepositorio = new Repositorio.RepositorioQuestao();
            _RepositorioSubmissao = new Repositorio.RepositorioSubmissao();
            _SubmissoesJulgadas = new List<Dominio.Submissao>();
            _SubmissoesNaoJulgadas = new List<Dominio.Submissao>();
        }

        private void AutoJudge_Load(object sender, EventArgs e)
        {
            var lista = _ContestRepositorio.Contests();
            cmbContest.DataSource = lista;
        }


        private void btnIniciar_Click(object sender, EventArgs e)
        {
            Dominio.Configuracao configuracao = new Dominio.Configuracao();
            if (configuracao.IsValido())
            {
                int idContest = (int)cmbContest.SelectedValue;
                if (idContest > 0)
                {
                    _Questoes = _QuestaoRepositorio.Questoes(idContest);
                    foreach (var item in _Questoes)
                    {
                        _QuestaoRepositorio.DownloadQuestao(item);
                    }

                    lstProblemas.DataSource = _Questoes;

                    cmbContest.Enabled = false;
                    timer2.Enabled = true;
                }
            }
        }

        private void cmbContest_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dominio.Contest contest = (Dominio.Contest)cmbContest.SelectedItem;
            if (contest != null)
            {
                lblInicio.Text = contest.DataInicio.ToString("dd-MM-yyyy hh:mm");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Dominio.Contest contest = (Dominio.Contest)cmbContest.SelectedItem;
            if (contest != null)
            {
                lblInicio.Text = contest.DataInicio.ToString("dd-MM-yyyy hh:mm");
                DateTime fim = contest.DataInicio.AddSeconds(contest.Tempo);
                if (fim > DateTime.Now)
                {
                    TimeSpan time = fim - DateTime.Now;
                    lblFinal.Text = time.Hours + ":" + time.Minutes + ":" + time.Seconds;
                }
                else
                {
                    lblFinal.Text = "Acabou";
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BuscarSubmissoes();
        }


        private void BuscarSubmissoes()
        {
            var submissoes = _RepositorioSubmissao.SubmissoesNovas();
            foreach (var item in submissoes)
            {
                if (_SubmissoesNaoJulgadas.Where(x => x.Id == item.Id).Count() == 0 && _SubmissoesJulgadas.Where(x => x.Id == item.Id).Count() == 0)
                    _SubmissoesNaoJulgadas.Add(item);
            }
            dataGridView1.DataSource = _SubmissoesNaoJulgadas;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Dominio.Contest contest = (Dominio.Contest)cmbContest.SelectedItem;
            if (_SubmissoesNaoJulgadas.Count > 0)
            {
                var submissao = _SubmissoesNaoJulgadas[0];
                _RepositorioSubmissao.DownloadArquivos(submissao);

                CompilarExecutar compiler = new CompilarExecutar();
                int retorno = compiler.Compilar(_Questoes.First(x => x.Id == submissao.IdProblema), submissao);
                if (retorno == 0)
                {
                    retorno = compiler.Executar(_Questoes.First(x => x.Id == submissao.IdProblema), submissao);
                    if (retorno == 0)
                    {
                        Comparador comparar = new Comparador();
                        retorno = comparar.Comparar(_Questoes.First(x => x.Id == submissao.IdProblema), submissao);
                        if (retorno == 0)
                            submissao.Resposta = Dominio.Resposta.Accept;
                        else
                            submissao.Resposta = Dominio.Resposta.WrongAnswer;
                    }
                    else if (retorno == 1)
                        submissao.Resposta = Dominio.Resposta.TimeLimit;
                    else
                        submissao.Resposta = Dominio.Resposta.SigSegv;
                }
                else
                    submissao.Resposta = Dominio.Resposta.CompilationError;

                submissao.Status = "judged";
                submissao.DiffTimeResposta = (int)DateTime.Now.Subtract(contest.DataInicio).TotalSeconds;

                _RepositorioSubmissao.Atualizar(submissao);

                _SubmissoesJulgadas.Add(submissao);
                _SubmissoesNaoJulgadas.RemoveAt(0);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _SubmissoesNaoJulgadas;
                gdvSubmissoesJulgadas.DataSource = null;
                gdvSubmissoesJulgadas.DataSource = _SubmissoesJulgadas;
            }
        }

        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            Configuracoes configurar = new Configuracoes();
            configurar.ShowDialog();
        }

        private void AutoJudge_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
