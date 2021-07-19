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

namespace Win_DisconnectedMimari
{
    public partial class KategoriForm : Form
    {
        public KategoriForm()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Server = (localdb)\MSSQLLocalDB;Database= KuzeyYeli;Integrated Security = true");

        private void KategoriForm_Load(object sender, EventArgs e)
        {
            KategoriListele();
        }

        private void KategoriListele()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select * from Kategoriler", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["KategoriID"].Visible = false;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format("Insert Kategoriler(KategoriAdi,Tanimi) values ('{0}','{1}')",
                txtKategoriAdi.Text, txtTanimi.Text);
            cmd.Connection = baglanti;
            baglanti.Open();
            int etk = cmd.ExecuteNonQuery();
            baglanti.Close();
            if (etk > 0)
            {
                MessageBox.Show("Kayıt Eklenmiştir");
                KategoriListele();
            }
            else
            {
                MessageBox.Show("Kayıt Eklenemedi");
            }
        }
    }
}
