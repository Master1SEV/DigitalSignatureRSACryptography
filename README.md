# -DigitalSignatureRSACryptography
Листинг кода функции
Генерация пары ключей 
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

        // функция создания пары ключей с использованием алгоритма RSA
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
Открытие пары ключей
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
Открытие публичного ключа 
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
Создание ЭЦП
private void button4_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText("public.xml"));
            publicKey = rsa.ExportParameters(false);

            MessageBox.Show("Публичный ключ открыт", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
Проверка ЭЦП
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

                // Проверка подписи выбранного файла
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
4. Образец созданной ЭЦП.
 
 
Образец публичного ключа
 
Образец приватного ключа
 

  
После чтения ключей создается цифровая подпись выбранного файла.

 
5. Результат проверки достоверной и недостоверной ЭЦП.
 

  	 
6.	Заключение по проделанной работе с выводами 
В результате проделанной работы были изучены:
•	основы технологии электронной цифровой подписи (ЭЦП), применяемой в компьютерных информационных системах на производстве, для подтверждения авторства, целостности и подлинности компьютерной документации в электронном виде;
•	Асимметричные криптографические алгоритмы;
•	основные возможности программирования, предоставляемые интегрированной средой разработки Visual Studio Community, сформированы навыки работы по разработке в Visual Studio Community приложения для создания ЭЦП с использованием алгоритма RSA.
Электронная цифровая подпись (ЭЦП) – это реквизит электронного документа, полученный в результате криптографического преобразования информации с использованием закрытого ключа подписи и позволяющий проверить отсутствие искажения информации в электронном документе с момента формирования подписи (целостность), принадлежность подписи владельцу сертификата ключа подписи (авторство), а в случае успешной проверки подтвердить факт подписания электронного документа (неотказуемость). В качестве подписываемого документа может быть использован любой файл. Подписанный файл создаётся из неподписанного путём добавления в него одной или более элек¬тронных подписей. ЭЦП добавляется к блоку данных и позволяет получателю блока проверить источник и целостность данных и защититься от их подделки.
ЭЦП обычно содержит допол¬нительную информацию, однозначно идентифицирующую авто¬ра подписанного документа: 
•	дату подписи;
•	срок окончания действия ключа данной подписи;
•	информацию о лице, подписавшем файл (Ф.И.О., долж¬ность, краткое наименование фирмы);
•	идентификатор подписавшего (имя открытого ключа).
Использование электронной подписи позволяет осуществить:
•	контроль целостности передаваемого документа: при любом случайном или преднамеренном изменении документа подпись станет недействительной, потому что вычислена она на основании исходного состояния документа и соответствует лишь ему; 
•	защиту от изменений (подделки) документа: гарантия выявления подделки при контроле целостности делает подделывание нецелесообразным в большинстве случаев; 
•	невозможность отказа от авторства. Так как создать корректную подпись можно, лишь зная закрытый ключ, а он должен быть известен только владельцу, то владелец не может отказаться от своей подписи под документом; 
•	доказательное подтверждение авторства документ. Так как создать корректную подпись можно, лишь зная закрытый ключ, а он должен быть известен только владельцу, то владелец пары ключей может доказать своё авторство подписи под документом. В зависимости от деталей определения документа могут быть подписаны такие поля, как «автор», «внесённые изменения», «метка времени» и т. д;
•	злоумышленник, перехватывающий все сообщения и знающий всю открытую информацию, не сможет найти исходное сообщение при больших значениях p и q.
