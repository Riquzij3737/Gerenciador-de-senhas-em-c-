using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slanostudioeuvejooqeufaço
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    
    public class funções
    {
        public static string Encrypt(string senha, string nome, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Concatenar os textos com um delimitador (ex: "|")
                            string concatenatedText = $"{senha} | {nome}";
                            swEncrypt.Write(concatenatedText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }


        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        public static void ExportEncryptedData(DataGridView dataGridView)
        {
            // Extrai os dados do DataGridView e armazena em uma string formatada
            StringBuilder dataBuilder = new StringBuilder();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    dataBuilder.AppendLine($"{row.Cells[0].Value},{row.Cells[1].Value}");
                }
            }

            // Dados em texto simples a serem criptografados
            string plainTextData = dataBuilder.ToString();

            // Chave e IV para AES-256 (32 bytes para chave, 16 bytes para IV)
            byte[] key = new byte[32];
            byte[] iv = new byte[16];

            // Gera a chave e o IV aleatórios
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }

            // Criptografa os dados
            byte[] encryptedData = EncryptStringToBytes_Aes(plainTextData, key, iv);

            // Usa um SaveFileDialog para selecionar o local de salvamento
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Database Files (*.db)|*.db";
                saveFileDialog.Title = "Salvar arquivo criptografado";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Salva a chave, IV e dados criptografados no arquivo
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        // Salva a chave e o IV primeiro
                        fs.Write(key, 0, key.Length);
                        fs.Write(iv, 0, iv.Length);

                        // Salva os dados criptografados
                        fs.Write(encryptedData, 0, encryptedData.Length);
                    }

                    MessageBox.Show("Dados salvos com sucesso!", "Gerenciador de senhas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


    }
}
