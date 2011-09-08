using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Boca
{
    public class CompilarExecutar
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SetErrorMode(int wMode);


        public int Compilar(Dominio.Questao questao, Dominio.Submissao submissao)
        {
            switch (submissao.Linguagem)
            {
                case Dominio.Linguagem.C:
                    return CompilarC(submissao);
                case Dominio.Linguagem.Cmaismais:
                    return CompilarCMaisMais(submissao);
                case Dominio.Linguagem.Java:
                    return CompilarJava(submissao);
                default:
                    throw new Exception("Linguagem do problema desconhecida");
            }
        }


        public int Executar(Dominio.Questao questao, Dominio.Submissao submissao)
        {
            Dominio.Configuracao configuracao = new Dominio.Configuracao();
            string arquivoIn = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, questao.ArquivoEntrada);
            string executavel = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, submissao.Id + ".exe");
            string saida = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, submissao.Id + ".out");

            string comando = string.Format("/c type \"{0}\"|{1} > \"{2}\"",arquivoIn, executavel, saida); //TODO Vou tentar usar o type

            return ExecutarPromptComandoComTempo(comando, questao.TimeLimit * 1000, submissao.Id.ToString());

        }

        private int CompilarC(Dominio.Submissao submissao)
        {
            Dominio.Configuracao configuracao = new Dominio.Configuracao();
            string arquivo = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, submissao.Id + ".c");
            string nome = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, submissao.Id + ".exe");

            string comando = string.Format("/c gcc \"{0}\" -o \"{1}\"", arquivo, nome);
            return ExecutarPromptComando(comando, configuracao.CaminhoGcc);
        }

        private int CompilarCMaisMais(Dominio.Submissao submissao)
        {
            Dominio.Configuracao configuracao = new Dominio.Configuracao();
            string arquivo = System.IO.Path.Combine(configuracao.CaminhoSalvarSubmissoes, submissao.Id + ".cpp");
            string nome = submissao.Id + ".exe";

            string comando = string.Format("/c g++ \"{0}\" -o \"{1}\"", arquivo, nome);
            return ExecutarPromptComando(comando, configuracao.CaminhoGmaismais);
        }

        private int CompilarJava(Dominio.Submissao submissao)
        {
            return 0;
        }

        private int ExecutarPromptComando(string comando, string pathExecutavel)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe", comando);
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.ErrorDialog = false;
            //info.EnvironmentVariables.Add("PATH", pathExecutavel);

            var processo = System.Diagnostics.Process.Start(info);
            processo.WaitForExit();
            string resposta = processo.StandardError.ReadToEnd();
            string saida = processo.StandardOutput.ReadToEnd();
            int codigo = processo.ExitCode;
            return codigo;
        }


        private int ExecutarPromptComandoComTempo(string argumentos, int tempoMilissegundos, string executavel)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe", argumentos);            
            info.CreateNoWindow = true;
            info.ErrorDialog = false;
            info.ErrorDialogParentHandle = System.IntPtr.Zero;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;


            int oldMode = SetErrorMode(3);
            var processo = System.Diagnostics.Process.Start(info);
            
            processo.WaitForExit(tempoMilissegundos);
            if (processo.HasExited == false)
            {
                processo.Kill();
                processo.WaitForExit();
                var processos = System.Diagnostics.Process.GetProcessesByName(executavel);
                if (processos.Length > 0)
                {
                    processos[0].Kill();
                    processos[0].Close();
                }
                
                SetErrorMode(oldMode);
                int cod = processo.ExitCode;
                return 1;
            }

            SetErrorMode(oldMode);
            processo.WaitForExit();
            int codigo = processo.ExitCode;
            return codigo;
        }      
    }
}
