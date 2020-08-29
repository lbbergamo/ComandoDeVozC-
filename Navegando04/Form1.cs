using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Microsoft.Speech.Recognition;
using System.Globalization;
using System.Threading;

namespace Navegando04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static CultureInfo ci = new CultureInfo("pt-BR");// linguagem utilizada
        static SpeechRecognitionEngine reconhecedor; // reconhecedor de voz
        SpeechSynthesizer resposta = new SpeechSynthesizer();// sintetizador de voz

        public string[] listaPalavras = { "proximo","anterior","fechar","ultimo","primeiro","leia"};

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'bDLivrosDataSet.TabLivro'. Você pode movê-la ou removê-la conforme necessário.
            this.tabLivroTableAdapter.Fill(this.bDLivrosDataSet.TabLivro);
            // TODO: esta linha de código carrega dados na tabela 'bDLivrosDataSet.TabLivro'. Você pode movê-la ou removê-la conforme necessário.
            this.tabLivroTableAdapter.Fill(this.bDLivrosDataSet.TabLivro);
            Init();
        }
        public void Init()
        {
            resposta.Volume = 100; 
            resposta.Rate = 3;
            Gramatica(); 
        }

        void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
        {
            string frase = e.Result.Text;
            if (frase.Equals("proximo"))
            {
                bindingNavigatorMoveNextItem.PerformClick();
                resposta.SpeakAsync("O comando funcionou");
                leia();
            }

            if (frase.Equals("anterior"))
            {
                bindingNavigatorMovePreviousItem.PerformClick();
                resposta.SpeakAsync("O comando funcionou");
                leia();
            }

            if (frase.Equals("ultimo"))
            {
                bindingNavigatorMoveLastItem.PerformClick();
                resposta.SpeakAsync("O comando funcionou");
            }

            if (frase.Equals("primeiro"))
            {
                bindingNavigatorMoveFirstItem.PerformClick();
                resposta.SpeakAsync("O comando funcionou");
            }
            if (frase.Equals("fechar"))
            {
                resposta.SpeakAsync("Até mais");

                Thread.Sleep(000);
                Close();
            }
            if (frase.Equals("leia"))
            {
                leia();
            }
        }

        public void Gramatica()
        {
            try
            {
                //reconhecedor = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-us"));
                reconhecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao integrar lingua escolhida:" + ex.Message);
            }

            // criacao da gramatica simples que o programa vai entender
            // usando um objeto Choices
            var gramatica = new Choices();
            gramatica.Add(listaPalavras); // inclui a gramatica criada

            // cria o construtor gramatical
            // e passa o objeto criado com as palavras
            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            // cria a instancia e carrega a engine de reconhecimento
            // passando a gramatica construida anteriomente
            try
            {
                var g = new Grammar(gb);

                try
                {
                    // carrega o arquivo de gramatica
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(g);
                    // registra a voz como mecanismo de entrada para o evento de reconhecimento
                    reconhecedor.SpeechRecognized += Sre_Reconhecimento;
                    reconhecedor.SetInputToDefaultAudioDevice(); // microfone padrao
                    resposta.SetOutputToDefaultAudioDevice(); // auto falante padrao
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple); // multiplo reconhecimento
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRO ao criar reconhecedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao criar a gramática: " + ex.Message);
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            leia();
        }

        private void leia()
        {
            resposta.SpeakAsync("codigo");
            resposta.SpeakAsync(codigoTextBox.Text);
            resposta.SpeakAsync("Titulo");
            resposta.SpeakAsync(tituloTextBox.Text);
            resposta.SpeakAsync("Autor");
            resposta.SpeakAsync(autorTextBox.Text);
            resposta.SpeakAsync("Editora");
            resposta.SpeakAsync(editoraTextBox.Text);
            resposta.SpeakAsync("Ano");
            resposta.SpeakAsync(anoTextBox.Text);
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            leia();
        }
    }
}
