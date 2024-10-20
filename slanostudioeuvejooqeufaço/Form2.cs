using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace slanostudioeuvejooqeufaço
{
    public partial class Form2 : Form
    {
        // Propriedade para armazenar a referência ao Form1
        private Form1 mainForm;

        // Construtor que recebe Form1 como parâmetro
        public Form2(Form1 form)
        {
            InitializeComponent();
            mainForm = form;
        }       

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Filter = "Salvar como arquivos de dados | *.db"
            };

            // Exibe o diálogo e verifica se o usuário clicou em "Salvar"
            if (checkBox1.Checked && fileDialog.ShowDialog() == DialogResult.OK)
            {
                // Verifica se os campos de texto estão preenchidos
                if (string.IsNullOrEmpty(Nome.Text) || string.IsNullOrEmpty(Senha.Text))
                {
                    DialogResult box = MessageBox.Show("Erro, não foi inserido os dados por completo, ainda desejas salvá-los?",
                                                       "Gerenciador de senhas", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (box != DialogResult.Yes)
                    {
                        MessageBox.Show("Operação cancelada.");
                        return;
                    }
                }

                string texto;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = 256; // Define o tamanho da chave para 256 bits (32 bytes)
                    aesAlg.GenerateKey();
                    aesAlg.GenerateIV();

                    byte[] key = aesAlg.Key;
                    byte[] iv = aesAlg.IV;

                    // Supondo que `funções.Encrypt` retorne uma string
                    texto = funções.Encrypt(Nome.Text, Senha.Text, key, iv);
                }

                // Salva o texto criptografado no arquivo
                File.WriteAllText(fileDialog.FileName, $"dados criptografados em aes-256 \n\n{texto}");
            }
            else
            {

                // Obtém os dados necessários (dados1, dados2, situacao)
                string dados1 = Nome.Text;
                string dados2 = Senha.Text;
                bool situacao = checkBox1.Checked;

                // Chama o método do Form1 para adicionar a linha
                mainForm.AddTabela(dados1, dados2, situacao);

                // Fecha o Form2 após adicionar os dados (opcional)
                this.Close();

            }
        }

    }
}
