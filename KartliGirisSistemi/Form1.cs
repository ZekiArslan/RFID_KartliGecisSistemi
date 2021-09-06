using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO.Ports;
using System.IO;

namespace KartliGirisSistemi
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection("server =.; Initial Catalog = KartliGirisSistemi; Integrated Security = True");
        SqlDataAdapter da;
        SqlCommand cmd;        
        string[] Birimler = { "Mühendis", "Teknisyen", "Hizmet", "Yönetim", "Torpilli" };
        public Form1()
        {
            InitializeComponent();
            serialPort1.BaudRate = 9600;
        }
        public void PortBaglanti()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = comboboxPort.Text;
                serialPort1.BaudRate = 9600;

                try
                {
                    serialPort1.Open();
                    lbl_BaglantiGiris.Text = "AKTİF.";
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                MessageBox.Show("Bağlantı zaten Aktif.");
            }
        }
        public void PortBaglantiKapat()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                lbl_BaglantiGiris.Text = "KAPALI";
            }

        }
        public void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string gelenVeri = serialPort1.ReadLine();
            txtKartID.Text = gelenVeri;
            txtGelenVeri.Text = gelenVeri;

        }
        public void Listele()
        {
            con.Open();
            da = new SqlDataAdapter("select * from tbl_Personel", con);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
            con.Close();
        }
        public void SorgulaGiris()
        {
            con = new SqlConnection("server =.; Initial Catalog = KartliGirisSistemi; Integrated Security = True");
            con.Open();
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select Adi,Soyadi,Birim,PersonelID from tbl_Personel WHERE KartID='" + txtKartID.Text + "'";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txtGirisAdi.Text = dr[0].ToString();
                txtGirisSoyadi.Text = dr[1].ToString();
                txtGirisBirim.Text = dr[2].ToString();
                txtGirisPersonel.Text = dr[3].ToString();
            }
            con.Close();

        }
        public void SorgulaCikis()
        {
            con = new SqlConnection("server =.; Initial Catalog = KartliGirisSistemi; Integrated Security = True");
            con.Open();
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select Adi,Soyadi,Birim,PersonelID from tbl_Personel WHERE KartID='" + txtKartID.Text + "'";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txtCikisAdi.Text = dr[0].ToString();
                txtCikisSoyadi.Text = dr[1].ToString();
                txtCikisBirim.Text = dr[2].ToString();
                txtCikisPersonel.Text = dr[3].ToString();
            }
            con.Close();
        }
        public void SorgulaPersonel()
        {
            con = new SqlConnection("server =.; Initial Catalog = KartliGirisSistemi; Integrated Security = True");
            con.Open();
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select Adi,Soyadi,Birim,PersonelID from tbl_Personel WHERE KartID='" + txtKartID.Text + "'";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txtAdi.Text = dr[0].ToString();
                txtSoyadi.Text = dr[1].ToString();
                comboBirim.Text = dr[2].ToString();
                txtPersonel.Text = dr[3].ToString();
            }
            con.Close();
        }
        public void Kaydet()
        {
            string sorgu = "insert into tbl_Personel(KartID,Adi,Soyadi,Birim,PersonelID) values(@p1,@p2,@p3,@p4,@p5)";
            cmd = new SqlCommand(sorgu, con);
            cmd.Parameters.AddWithValue("@p1", txtKartID.Text);
            cmd.Parameters.AddWithValue("@p2", txtAdi.Text);
            cmd.Parameters.AddWithValue("@p3", txtSoyadi.Text);
            cmd.Parameters.AddWithValue("@p4", comboBirim.Text);
            cmd.Parameters.AddWithValue("@p5", txtPersonel.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Listele();
            MessageBox.Show("Kayıt Yapıldı.");
        }
        public void Sil()
        {
            string sorgu = "delete from tbl_Personel WHERE KartID=@ID";
            cmd = new SqlCommand(sorgu, con);
            cmd.Parameters.AddWithValue("@ID", txtKartID.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Listele();
            MessageBox.Show("Kayıt Silindi.");
        }
        public void Guncelle()
        {
            string sorgu = "update tbl_Personel set Adi=@p1,Soyadi=@p2,Birim=@p3,PersonelID=@p4 WHERE KartID=@ID";
            cmd = new SqlCommand(sorgu, con);
            cmd.Parameters.AddWithValue("@ID", txtKartID.Text);
            cmd.Parameters.AddWithValue("@p1", txtAdi.Text);
            cmd.Parameters.AddWithValue("@p2", txtSoyadi.Text);
            cmd.Parameters.AddWithValue("@p3", comboBirim.Text);
            cmd.Parameters.AddWithValue("@p4", txtPersonel.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Listele();
            MessageBox.Show("Güncelleme Yapıldı.");
        }
        public void ComboDoldur()
        {
            comboboxPort.DataSource = SerialPort.GetPortNames();
            for (int i = 0; i < Birimler.Length; i++)
            {
                comboBirim.Items.Add(Birimler[i]);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Listele();
            ComboDoldur();
            txtKartID.Enabled = false;
            radioButton5.Checked = true;

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Adı", 80);
            listView1.Columns.Add("Soyadı", 80);
            listView1.Columns.Add("Giriş Saati", 200);

            listView2.View = View.Details;
            listView2.FullRowSelect = true;
            listView2.Columns.Add("Adı", 80);
            listView2.Columns.Add("Soyadı", 80);
            listView2.Columns.Add("Giriş Saati", 200);
        }
        private void btn_Baglan_Click(object sender, EventArgs e)
        {
            PortBaglanti();
        }

        private void txtGelenVeri_TextChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked==true)
            {
                SorgulaGiris();
                
            }
            if (radioButton6.Checked==true)
            {
                SorgulaCikis();
                
            }
            if (radioButton5.Checked==false&&radioButton6.Checked==false)
            {
                MessageBox.Show("Giriş Çıkış İşlemlerinden Birini Seçmelisiniz.");
            }

            
        }
        private void txtKartID_TextChanged(object sender, EventArgs e)
        {
            SorgulaPersonel();
        }

        private void btnIslemYap_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked==true)
            {
                Kaydet();
                txtKartID.Text = "";
                txtAdi.Text = "";
                txtSoyadi.Text = "";
                comboBirim.Text = "";
                txtPersonel.Text = "";
            }
            if (radioButton2.Checked==true)
            {
                Guncelle();
                txtKartID.Text = "";
                txtAdi.Text = "";
                txtSoyadi.Text = "";
                comboBirim.Text = "";
                txtPersonel.Text = "";
            }
            if (radioButton3.Checked==true)
            {
                Sil();
                txtKartID.Text = "";
                txtAdi.Text = "";
                txtSoyadi.Text = "";
                comboBirim.Text = "";
                txtPersonel.Text = "";
            }
            MessageBox.Show("Lütfen İşlem Seçin.");
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
            }
        }

        private void btnGirisOnay_Click(object sender, EventArgs e)
        {
            serialPort1.Write("+");
            serialPort1.Write(txtGirisAdi.Text);
            serialPort1.Write(" ");
            serialPort1.Write(txtGirisSoyadi.Text);
            serialPort1.Write("/");
            serialPort1.Write("GIREBILIRSINIZ");

            string ad = txtGirisAdi.Text;
            string soyad = txtGirisSoyadi.Text;
            string saat = DateTime.Now.ToString();
            string[] data = { ad, soyad, saat };
            listView1.Items.Add(new ListViewItem(data));

        }

        private void btnCikisOnay_Click(object sender, EventArgs e)
        {
            serialPort1.Write("+");
            serialPort1.Write(txtCikisAdi.Text);
            serialPort1.Write(" ");
            serialPort1.Write(txtCikisSoyadi.Text);
            serialPort1.Write("/");
            serialPort1.Write("CIKABİLİRSİNİZ");

            string ad = txtCikisAdi.Text;
            string soyad = txtCikisSoyadi.Text;
            string saat = DateTime.Now.ToString();
            string[] data = { ad, soyad, saat };
            listView2.Items.Add(new ListViewItem(data));

        }

        private void btnGirisReddet_Click(object sender, EventArgs e)
        {
            serialPort1.Write("-");
            serialPort1.Write(txtGirisAdi.Text);
            serialPort1.Write(" ");
            serialPort1.Write(txtGirisSoyadi.Text);
            serialPort1.Write("/");
            serialPort1.Write(txtGirisRed.Text);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked==true)
            {
                btnCikisOnay.Enabled = false;
                btnGirisOnay.Enabled = true;
                btnGirisReddet.Enabled = true;
                txtCikisAdi.Text = "";
                txtCikisSoyadi.Text = "";
                txtCikisBirim.Text = "";
                txtCikisPersonel.Text = "";
            }
            if (radioButton6.Checked == true)
            {
                btnCikisOnay.Enabled = true;
                btnGirisOnay.Enabled = false;
                btnGirisReddet.Enabled = false;
                txtGirisAdi.Text = "";
                txtGirisSoyadi.Text = "";
                txtGirisBirim.Text = "";
                txtGirisPersonel.Text = "";
            }

        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                MessageBox.Show("Bağlantı Aktif.");
            if (!serialPort1.IsOpen)
                MessageBox.Show("Bağlantı Aktif Değil.");
        }

        private void btn_BaglantiKes_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                lbl_BaglantiGiris.Text = "Kapatıldı.";
            }
            else
            {
                MessageBox.Show("Bağlantı Zaten Kapalı.");
            }
        }
    }
}
