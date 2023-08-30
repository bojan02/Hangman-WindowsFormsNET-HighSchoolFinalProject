using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Igra_pogađanja_reči
{
    public partial class Form1 : Form
    {
        StreamReader srd = new StreamReader("words.txt");        
        List<string> ucitaneReci = new List<string>();
        List<Igrica> iskoristenoSlovo = new List<Igrica>();
        SoundPlayer zvukDobijenaIgra = new SoundPlayer(@"pobeda.wav");
        SoundPlayer zvukIzgubljenaIgra = new SoundPlayer(@"izgubljena.wav");
        SoundPlayer zvukPogodjenoSlovo = new SoundPlayer(@"pogodjena.wav");
        SoundPlayer zvukPromasenoSlovo = new SoundPlayer(@"greska.wav");
        Random randBr = new Random();
        PictureBox[] slike = new PictureBox[10];
        string jednaRec;
        string reciIzTxt = "";
        int izabranaRec;
        int topPozCrte = 400, leftPozCrte = 10;
        int topPozSlova = 370, leftPozSlova = 10;
        char unesenoSlovo;
        Label[] slova;        
        int brojPogodjenih = 0, brojGresaka = 0;
        int brojKaraktera;
        char[] svaSlova = new char[30];
        int p = 0;
        public Form1()
        {
            InitializeComponent();

            while ((jednaRec = srd.ReadLine()) != null)
            {
                jednaRec = jednaRec.ToUpper();
                if (jednaRec.Length > 0)
                {
                    reciIzTxt += jednaRec + "\n";
                    ucitaneReci.Add(jednaRec);
                }
            }
            srd.Close();
            int brRijeci = ucitaneReci.Count();

            izabranaRec = randBr.Next(0, brRijeci);
            lblResenja.Text = Convert.ToString(izabranaRec);
            brojKaraktera = ucitaneReci[izabranaRec].Length - brojRazmakaIzmedjuReci();

            slike[0] = pictureBox1;
            slike[1] = pictureBox2;
            slike[2] = pictureBox3;
            slike[3] = pictureBox4;
            slike[4] = pictureBox5;
            slike[5] = pictureBox6;
            slike[6] = pictureBox7;
            slike[7] = pictureBox8;
            slike[8] = pictureBox9;
            slike[9] = pictureBox10;

            sakrijSlike();
            crtajLinije();
            ispisSlova();
        }

        private int brojRazmakaIzmedjuReci()
        {
            string a = ucitaneReci[izabranaRec];
            int brRazmaka = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == ' ')
                    brRazmaka++;
            }
            return brRazmaka;
        }

        private void sakrijSlike()
        {
            for (int i = 0; i < 10; i++)
            {
                slike[i].Visible = false;
            }
        }

        private void crtajLinije()
        {
            Label[] labels = new Label[ucitaneReci[izabranaRec].Length];
            string s = ucitaneReci[izabranaRec];
            lblResenja.Text = s;
            for (int i = 0; i < s.Length; i++)
            {
                labels[i] = new Label();
                if (s[i] != ' ')
                    labels[i].Text = "‾";
                else
                    labels[i].Text = " ";
                labels[i].Font = new Font("Arial", 30);
                labels[i].AutoSize = true;
                labels[i].Top = topPozCrte;
                labels[i].Left = leftPozCrte;
                labels[i].Height = 1;
                this.Controls.Add(labels[i]);
                leftPozCrte += 50;
            }
        }

        private void ispisSlova()
        {
            slova = new Label[ucitaneReci[izabranaRec].Length];
            string s = ucitaneReci[izabranaRec];

            for (int i = 0; i < ucitaneReci[izabranaRec].Length; i++)
            {
                if (s[i] != ' ')
                {
                    slova[i] = new Label();
                    slova[i].Font = new Font("Arial", 20);
                    slova[i].AutoSize = true;
                    slova[i].Top = topPozSlova;
                    slova[i].Left = leftPozSlova;
                    this.Controls.Add(slova[i]);
                }
                leftPozSlova += 50;
            }
        }

        private void povecajSlovo(object sender, KeyEventArgs e)
        {
            lblMessidzBox.BackColor = Color.Gray;
            lblMessidzBox.Text = "";
            textBox1.Text = textBox1.Text.ToUpper();
            Igrica a = new Igrica();
            a.x = textBox1.Text;
            textBox1.Text = "";
            a.x = a.x.ToUpper();
            if (!a.ValidacijaIskoristenihSlova(iskoristenoSlovo))
            {
                lblMessidzBox.BackColor = Color.Red;
                lblMessidzBox.Text = "Slovo je već bilo u upotrebi!";
            }
            else
            {
                if (a.x.Length == 1)
                {
                    unesenoSlovo = char.Parse(a.x);
                    proveraKoristenosti();
                    iskoristenoSlovo.Add(a);
                    if (!koristenoUReci())
                    {
                        svaSlova[p] = unesenoSlovo;
                        p++;
                    }
                    unesenoSlovo = '\0';
                    textBox1.Text = "";
                }
                else
                {
                    unesenoSlovo = '\0';
                    textBox1.Text = "";
                }

                if (brojPogodjenih == brojKaraktera)
                {
                    textBox1.Enabled = false;
                    lblMessidzBox.Text = "POBEDA";
                    zvukDobijenaIgra.Play();

                    slike[0].Visible = true;
                    slike[1].Visible = true;
                    slike[2].Visible = true;
                    slike[3].Visible = false;
                    slike[4].Visible = true;
                    slike[5].Visible = false;
                    slike[6].Visible = false;
                    slike[7].Visible = false;
                    slike[8].Visible = false;
                    slike[9].Visible = true;
                }
                else
                {
                    crtajNaGresku();
                }
            }
            string svaIskoristenaSlova = "";
            foreach (Igrica clan in iskoristenoSlovo)
            {
                svaIskoristenaSlova += clan.x;
            }
            lblPotrosenaSlova.Text = svaIskoristenaSlova;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lblResenja.Visible = !lblResenja.Visible;
        }

        private void restartIgre(object sender, EventArgs e)
        {
            var x = MessageBox.Show(null, "Želite li ponovno pokrenuti igru?", "Restart?", MessageBoxButtons.YesNo);
            if (x == DialogResult.Yes)
                Application.Restart();
        }

        private void izlazIgre(object sender, EventArgs e)
        {
            var x = MessageBox.Show(null, "Želite li izaći iz igre?", "Izađi?", MessageBoxButtons.YesNo);
            if (x == DialogResult.Yes)
                Application.Exit();
        }

        private void crtajNaGresku()
        {
            for (int i = 0; i < brojGresaka; i++)
            {
                slike[i].Visible = true;
            }

            if (brojGresaka == 9)
            {
                lblMessidzBox.Text = "KRAJ IGRE";
                textBox1.Enabled = false;
                zvukIzgubljenaIgra.Play();
            }
        }

        private void proveraKoristenosti()
        {
            string s = ucitaneReci[izabranaRec];
            int j = 0;

            if (koristenoUReci() == false)
            {
                for (int i = 0; i < ucitaneReci[izabranaRec].Length; i++)
                {
                    if (s[i] == unesenoSlovo)
                    {
                        slova[i].Text = unesenoSlovo.ToString();
                        brojPogodjenih++;
                        j++;
                    }
                }
            }
            if (j == 0)
            {
                brojGresaka++;
                zvukPromasenoSlovo.Play();
            }
            else
                zvukPogodjenoSlovo.Play();
        }

        private bool koristenoUReci()
        {
            for (int i = 0; i < 30; i++)
            {
                if (unesenoSlovo == svaSlova[i])
                    return true;
            }
            return false;
        }

        TextBox tbOp1 = new TextBox();
        RichTextBox rtOp1 = new RichTextBox();
        Form opcije = new Form();
        private void opcijeMeni(object sender, EventArgs e)
        {
            opcije.Width = 550;
            opcije.Height = 400;
            opcije.Left = 100;
            opcije.Top = 100;
            opcije.ControlBox = false;
            opcije.Show();
            opcije.Text = "Opcije dodavanja i pregledavanja reči";
            opcije.MaximizeBox = true;

            opcije.Controls.Add(tbOp1);
            tbOp1.Left = 50;
            tbOp1.Top = 50;

            rtOp1.Left = 50;
            rtOp1.Top = 80;
            rtOp1.Width = 80;
            rtOp1.Height = 200;
            rtOp1.Width = 300;
            opcije.Controls.Add(rtOp1);
            rtOp1.Text = reciIzTxt;
            rtOp1.Text = rtOp1.Text.ToUpper();

            Button btnOp1 = new Button();
            btnOp1.Top = 50;
            btnOp1.Left = 200;
            btnOp1.Text = "DODAJ";
            opcije.Controls.Add(btnOp1);
            btnOp1.Click += new EventHandler(dodajNovuRec);

            Button btnOp2 = new Button();
            btnOp2.Top = 50;
            btnOp2.Left = 400;
            btnOp2.Text = "IZLAZ";
            opcije.Controls.Add(btnOp2);
            btnOp2.Click += new EventHandler(zatvori);
        }

        private void zatvori(object s, EventArgs e)
        {
            opcije.Hide();
        }
        private void dodajNovuRec(object s, EventArgs e)
        {
            string novaRec = tbOp1.Text;
            StreamWriter sw = new StreamWriter("words.txt", true);
            sw.WriteLine(novaRec);
            sw.Close();
            
            tbOp1.Text = "";
            
            reciIzTxt += novaRec + "\n";
            rtOp1.Text = reciIzTxt;
            rtOp1.Text = rtOp1.Text.ToUpper();
        }
    }
}