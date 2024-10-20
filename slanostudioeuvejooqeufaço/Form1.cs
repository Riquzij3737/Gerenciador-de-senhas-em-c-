using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slanostudioeuvejooqeufaço
{   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }       

        // Método público para adicionar dados ao DataGridView
        public void AddTabela(string dados1, string dados2, bool situacao)
        {
            if (string.IsNullOrEmpty(dados1) || string.IsNullOrEmpty(dados2))
            {
                MessageBox.Show("ERRO: O salvamento não pode ser efetuado pois os dados não foram inseridos corretamente");
                return;
            }

            // Adiciona os dados ao DataGridView
            dataGridView1.Rows.Add(dados1, dados2, situacao);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }                

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Senhas.ReadOnly == true && Nomes.ReadOnly == true)
            {
                Senhas.ReadOnly = false;
                Nomes.ReadOnly = false;
                label2.BackColor = System.Drawing.Color.Green;
                label2.Text = "EDIÇÃO: ON";
            } else {
                Nomes.ReadOnly = true;
                Senhas.ReadOnly = true;
                label2.BackColor = System.Drawing.Color.Red;
                label2.Text = "EDIÇÃO: OFF";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Verifica se há uma linha selecionada no DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Remove a linha selecionada
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma linha para remover.", "Nenhuma linha selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            funções.ExportEncryptedData(dataGridView1 as DataGridView);
        }
    }
}
