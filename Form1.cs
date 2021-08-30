using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace WindowsFormsApp3
{
    
    public partial class Form1 : Form
    {
        RSAParameters publicKey;
        RSAParameters privateKey;

        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = false;
            button5.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        // Выбор режима 
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                radioButton1.Checked = true;
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                // проверка подписи
                button4.Visible = false;
                button5.Visible = false;
            }
            else
            {
                radioButton1.Checked = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                // проверка подписи
                button4.Visible = true;
                button5.Visible = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        // GenPairKeyButton
        private void button1_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            rsa.PersistKeyInCsp = false;
            publicKey = rsa.ExportParameters(false);
            privateKey = rsa.ExportParameters(true);

            File.WriteAllText("public.xml", rsa.ToXmlString(false));
            File.WriteAllText("private.xml", rsa.ToXmlString(true));
            MessageBox.Show("Ключи созданы и сохранены в файлы public.xml и private.xml", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);

            rsa.PersistKeyInCsp = false;

            rsa.FromXmlString(File.ReadAllText("public.xml"));
            rsa.FromXmlString(File.ReadAllText("private.xml"));

            publicKey = rsa.ExportParameters(false);
            privateKey = rsa.ExportParameters(true);
            MessageBox.Show("Ключи открыты", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                FileStream fStream = File.OpenRead(filename);

                //код создания подписи выбранного файла
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(privateKey);

                fStream.Position = 0;

                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] hash = sha256.ComputeHash(fStream);
                fStream.Close();

                byte[] sign = rsa.SignHash(hash, "SHA256");

                richTextBox1.Text = BitConverter.ToString(sign);

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText("public.xml"));
            publicKey = rsa.ExportParameters(false);

            MessageBox.Show("Публичный ключ открыт", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Подпись не указана");
                return;
            }
            try
            {
                string[] strSign = richTextBox1.Text.Split('-');
                byte[] sign = new byte[strSign.Length];
                for (int i = 0; i < strSign.Length; i++)
                {
                    uint num = uint.Parse(strSign[i], System.Globalization.NumberStyles.AllowHexSpecifier);
                    sign[i] = Convert.ToByte(num);
                }
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = openFileDialog.FileName;
                    FileStream fStream = File.OpenRead(filename);

                //код проверки подписи выбранного файла
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
                rsa.ImportParameters(publicKey);

                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] hash = sha256.ComputeHash(fStream);
                fStream.Close();
                
                

                if (rsa.VerifyHash(hash, "SHA256", sign))
                {
                    MessageBox.Show("Подпись достоверна", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Подпись не достоверна", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
