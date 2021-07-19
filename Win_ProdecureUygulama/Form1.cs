using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_ProdecureUygulama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"server=(localdb)\MSSQLLocalDB;Database = KuzeyYeli;Integrated Security = true;");
        

        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("UrunEkle",baglanti);//prosedurun ismini ve baglantiyi verdik
            cmd.CommandType = CommandType.StoredProcedure;//cmd nin bir prosedur olduğunu tanıtmış olduk yoksa yukarıdakini yazpı bıraksaydın onu bir sorgu kmutu zanneder
            cmd.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
            cmd.Parameters.AddWithValue("@fiyat", nudFiyat.Value);
            cmd.Parameters.AddWithValue("@stok", nudStok.Value);

            baglanti.Open();
            int etk = cmd.ExecuteNonQuery();
            baglanti.Close();
            if (etk>0)
            {
                MessageBox.Show("Kayıt Eklendi");
                UrunListele();
            }
            else
            {
                MessageBox.Show("Kayıt Eklenirken Hata oluştu");
            }
        }

        private void UrunListele()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select * from Urunler where Sonlandi = 0", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UrunListele();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                SqlCommand cmd = new SqlCommand("UrunSil",baglanti);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@urunId", dataGridView1.CurrentRow.Cells["UrunID"].Value);
                baglanti.Open();
                int etk = cmd.ExecuteNonQuery();
                baglanti.Close();
                if (etk>0)
                {
                    MessageBox.Show("Kayıt Silinmiştir");
                    UrunListele();
                }
                else
                {
                    MessageBox.Show("Kayıt silinirken hata oluştu");
                }

            }
        }
    }
}
