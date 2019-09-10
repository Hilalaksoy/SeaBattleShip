using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBattleShip1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }        
        int toplam_oyuncu_gemi = 20;
        int toplam_dusman_gemi = 20;
        int toplam_oyuncu_skor = 0;
        int toplam_dusman_skor = 0;
        int rounds = 40;
        Random rnd = new Random();
        List<Button> oyuncuButonları;
        List<Button> bilgisayarButonları;

        private void Form1_Load(object sender, EventArgs e)
        {
            OyunAlaniOyuncu();
            OyunAlaniBilgisayar();
            lblRound.Text = rounds.ToString();
            oyuncuButonları = new List<Button> { };
            oyuncuButonları.AddRange(flwPanelOyuncu.Controls.OfType<Button>());

            bilgisayarButonları = new List<Button> { };
            bilgisayarButonları.AddRange(flwPanelBilgisayar.Controls.OfType<Button>());

            foreach (Control item in flwPanelBilgisayar.Controls)
            {
                item.Enabled = false;

            }
            for (int i = 0; i < bilgisayarButonları.Count; i++)
            {
                bilgisayarButonları[i].Tag = null;
                //Düşman listesine yerlerinin isimlerini kaydediyor.
                cmbDusmanListesi.Items.Add(bilgisayarButonları[i].Text);
            }
            
        }

        private void OyunAlaniOyuncu()
        {
            String[] harfler = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            for (int x = 0; x < harfler.Length; x++)
            {
                for (int i = 0; i < 10; i++)
                {

                    Button btnOyuncu = new Button();
                    btnOyuncu.Width = 40;
                    btnOyuncu.Height = 40;
                    btnOyuncu.FlatStyle = FlatStyle.Flat;
                    btnOyuncu.FlatAppearance.BorderColor = Color.White;
                    btnOyuncu.BackColor = Color.CornflowerBlue;
                    btnOyuncu.Click += BtnOyuncu_Click;
                    btnOyuncu.Text = string.Format("{0}{1}", harfler[x], (i + 1));
                    btnOyuncu.Name = string.Format("{0}{1}", harfler[x], (i + 1));
                    flwPanelOyuncu.Controls.Add(btnOyuncu);
                }
            }
        }
        private void OyunAlaniBilgisayar()
        {
            
            String[] harfler = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            for (int x = 0; x < harfler.Length; x++)
            {
                for (int i = 0; i < 10; i++)
                {

                    Button btnBilgisayar = new Button();
                    btnBilgisayar.Width = 40;
                    btnBilgisayar.Height = 40;
                    btnBilgisayar.FlatStyle = FlatStyle.Flat;
                    btnBilgisayar.FlatAppearance.BorderColor = Color.White;
                    btnBilgisayar.BackColor = Color.CornflowerBlue;
                    btnBilgisayar.Click += BtnBilgisayar_Click; ;

                    btnBilgisayar.Text = string.Format("{0}{1}", harfler[x], (i + 1));
                    btnBilgisayar.Name = string.Format("{0}{1}", harfler[x], (i + 1));
                    flwPanelBilgisayar.Controls.Add(btnBilgisayar);
                }
            }
        }

        //Oyuncunun gemileri yerleştirmesi
        private void BtnOyuncu_Click(object sender, EventArgs e)
        {
            if (toplam_oyuncu_gemi > 0)
            {
                Button basilanYer = (Button)sender;
                basilanYer.Enabled = false;
                basilanYer.Tag = "oyuncuGemisi";
                basilanYer.BackColor = Color.Navy;
                toplam_oyuncu_gemi--;
            }
            if (toplam_oyuncu_gemi == 0)
            {
                btnDusmanaSaldir.Enabled = true;
                foreach (Button item in bilgisayarButonları)
                {
                    item.Enabled = true;
                }
                btnDusmanaSaldir.BackColor = Color.OrangeRed;            
            }

        }

        //Bilgisayarın gemileri yerleştirmesi
        private void BtnBilgisayar_Click(object sender, EventArgs e)
        {
            int index = rnd.Next(bilgisayarButonları.Count);

            if (bilgisayarButonları[index].Enabled == true && bilgisayarButonları[index].Tag == null)
            {
                bilgisayarButonları[index].Tag = "dusmanGemisi";
                toplam_dusman_gemi--;
            }
            else
            {
                index = rnd.Next(bilgisayarButonları.Count);
            }
            if(toplam_dusman_gemi < 1)
            {
                //gemi yerleştirme bitsin.
                dusmanYer.Stop();                
            }
        }
      
        //Bilgisayara karşı yapılan hamle
        private void btnDusmanaSaldir_Click(object sender, EventArgs e)
        {
            if(cmbDusmanListesi.Text != "")
            {
                var atakYeri = cmbDusmanListesi.Text;
                
                int index = bilgisayarButonları.FindIndex(a => a.Name == atakYeri);
                
                if(bilgisayarButonları[index].Enabled && rounds > 0)
                {
                    rounds--;
                    lblRound.Text = rounds.ToString();
                    if (bilgisayarButonları[index].Tag == "dusmanGemisi")
                    {
                        bilgisayarButonları[index].Enabled = false;
                        bilgisayarButonları[index].BackColor = Color.DarkOrange;
                        toplam_oyuncu_skor++;
                        lblOyuncuSkor.Text = toplam_oyuncu_skor.ToString();
                        dusmanOyunTimer.Start();
                    }
                    else
                    {
                        bilgisayarButonları[index].Enabled = false;
                        bilgisayarButonları[index].BackColor = Color.Aquamarine;
                        dusmanOyunTimer.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("ATAK YERİ SEÇİNİZ ...");
            }
        }

        //Oyuncuya karşı yapılan hamle
        private void btnOyuncuyaSaldir_Click(object sender, EventArgs e)
        {
            if (oyuncuButonları.Count > 0 && rounds > 0)
            {
                rounds--;
                lblRound.Text = rounds.ToString();
                int index = rnd.Next(oyuncuButonları.Count);
                if (oyuncuButonları[index].Tag == "oyuncuGemisi")
                {
                    oyuncuButonları[index].BackColor = Color.DarkOrange;
                    lblDusmanHareketi.Text = oyuncuButonları[index].Text;
                    oyuncuButonları[index].Enabled = false;
            
                    oyuncuButonları.RemoveAt(index);
                    toplam_dusman_skor++;
                    lblDusmanSkor.Text = toplam_dusman_skor.ToString();
                    dusmanOyunTimer.Stop();
                }
                else
                {
                    //Yanlış seçim yapılan yerler.
                    oyuncuButonları[index].BackColor = Color.Aquamarine;
                    lblDusmanHareketi.Text = oyuncuButonları[index].Text;
                    oyuncuButonları[index].Enabled = false;  
                    
                    oyuncuButonları.RemoveAt(index);
                    dusmanOyunTimer.Stop();
                }
            }
            if( rounds < 1 || toplam_oyuncu_skor >19 || toplam_dusman_skor > 19)
            {
                if(toplam_oyuncu_skor > toplam_dusman_skor)
                {
                    MessageBox.Show("KAZANDIN!!..");
                    this.Close();
                }
                if(toplam_oyuncu_skor == toplam_dusman_skor)
                {
                    MessageBox.Show("Berabere!!..");
                    this.Close();
                }
                if(toplam_dusman_skor > toplam_oyuncu_skor)
                {
                    MessageBox.Show("KAYBETTİN !!..");
                    
                    this.Close();
                }
            }
        }
    }
}
