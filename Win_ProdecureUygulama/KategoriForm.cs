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
    public partial class KategoriForm : Form
    {
        public KategoriForm()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"server=(localdb)\MSSQLLocalDB;Database = KuzeyYeli;Integrated Security = true;");

        private void KategoriForm_Load(object sender, EventArgs e)
        {
            KategoriListele();
        }

        private void KategoriListele()
        {
            SqlDataAdapter adp = new SqlDataAdapter("prc_KategoriListele", baglanti);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("prc_KategoriEkle", baglanti);
            komut.CommandType = CommandType.StoredProcedure;
            komut.Parameters.AddWithValue("@adi", txtAdi.Text);
            komut.Parameters.AddWithValue("@tanim", txtTanim.Text);
            baglanti.Open();
            int etk = komut.ExecuteNonQuery();
            baglanti.Close();
            if (etk > 0)
            {
                MessageBox.Show("Kayıt eklenmiştir");
                KategoriListele();
            }
            else
            {
                MessageBox.Show("Ekleme yapılırken hata oluştu");
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                SqlCommand komut = new SqlCommand("prc_KategoriSil", baglanti);
                komut.CommandType = CommandType.StoredProcedure;
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KategoriId"].Value);
                komut.Parameters.AddWithValue("@kId", id);
                baglanti.Open();
                int etk = komut.ExecuteNonQuery();
                baglanti.Close();
                if (etk > 0)
                {
                    MessageBox.Show("Kayıt Silinmiştir");
                    KategoriListele();
                }
                else
                {
                    MessageBox.Show("Kayıt silinirken hata oluştu");
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("prc_KategoriGuncelle",baglanti);
            DataGridViewRow row = dataGridView1.CurrentRow;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", row.Cells["KategoriId"].Value);
            cmd.Parameters.AddWithValue("@adi", row.Cells["KategoriAdi"].Value);
            cmd.Parameters.AddWithValue("@tanim", row.Cells["Tanimi"].Value);
            baglanti.Open();
            int etk = cmd.ExecuteNonQuery();
            baglanti.Close();
            if (etk>0)
            {
                MessageBox.Show("Güncelledim Baba");
                KategoriListele();
            }
            else
            {
                MessageBox.Show("Başaramadık abi");
            }

        }
    }
}
