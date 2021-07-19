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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Server = (localdb)\MSSQLLocalDB;Database = KuzeyYeli;Integrated Security = true");

        private void Form1_Load(object sender, EventArgs e)
        {
            UrunListesi();
        }

        private void UrunListesi()
        {
            //Disconnnected mimari yöntemi ile veri listeleme işlemi 
            SqlDataAdapter adp = new SqlDataAdapter("select * from Urunler where Sonlandi = 0", baglanti); //command ile tek tek open closse demeye gerek yok adapter ile bu şekildede iş yapabilirsin
            DataTable dt = new DataTable(); //Datatable içerisinde tablo tutan bir objedir
            adp.Fill(dt); //adapter doldursun kimi dt yi
            dataGridView1.DataSource = dt; //datagridview data kaynağına yukarıdaki hazırladığımız tabloyu verdik 
            //datagridviewın otomatik olarak kolonları doldurup gösterebilmesi autogeneratecolumns özelliğinin true olmasından dolayı
            dataGridView1.Columns["UrunID"].Visible = false; //ID kolonunun boşu boşuna olmasını istemediğim için visiible özelliğini false yapıyorum dikkat et datasource dt yi verdikten sonra bunu yapıyoruz önce yapsaydık olmazdı
            dataGridView1.Columns["KategoriID"].Visible = false;
            dataGridView1.Columns["TedarikciID"].Visible = false;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string adi = txtUrunAdi.Text;
            decimal fiyat = nudFiyat.Value;
            decimal stok = nudStok.Value;
            if (adi == "" || fiyat == null || stok == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz");
            }
            SqlCommand komut = new SqlCommand();
            komut.CommandText = string.Format("insert Urunler(UrunAdi,Fiyat,Stok) values ('{0}',{1},{2})", adi, fiyat, stok);
            komut.Connection = baglanti;
            baglanti.Open();
            int etkilenen = komut.ExecuteNonQuery();//komutu sql e yazıp seçip çalıştırıyor eğer işlem başarılı ise 0 dan büyük değilse 0 döner
            if (etkilenen > 0)
            {
                MessageBox.Show("Kayıt Başarılı bir şekilde eklendi");
                UrunListesi();
            }
            else
            {
                MessageBox.Show("Kayıt ekleme sırasında bir hata meydana geldi");
            }

            baglanti.Close();
        }

        private void btnKategoriler_Click(object sender, EventArgs e)
        {
            KategoriForm kf = new KategoriForm();
            kf.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //datagridviewdan seçili satırı alma işlemi
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["UrunAdi"].Value.ToString();
            nudFiyat.Value = (decimal)dataGridView1.CurrentRow.Cells["Fiyat"].Value;
            nudStok.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Stok"].Value);
            txtUrunAdi.Tag = dataGridView1.CurrentRow.Cells["UrunID"].Value;

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.CommandText = string.Format("Update Urunler set UrunAdi='{0}' , Fiyat={1} , Stok={2} where UrunID={3}",
                txtUrunAdi.Text, nudFiyat.Value, nudStok.Value, txtUrunAdi.Tag);
            komut.Connection = baglanti;
            baglanti.Open();

            try
            {
                int etk = komut.ExecuteNonQuery();
                if (etk > 0)
                {
                    MessageBox.Show("Kayıt Günclelendi");
                    UrunListesi();
                }
                else
                {
                    MessageBox.Show("Kayıt güncellenirken HATA oluştu");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Kayıt güncellenirken HATA oluştu");

            }
            finally
            {
                baglanti.Close();
            }


        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow!=null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UrunID"].Value);
                SqlCommand cmd = new SqlCommand(string.Format("delete Urunler where UrunID={0}", id), baglanti);
                baglanti.Open();
                int etk = cmd.ExecuteNonQuery();
                baglanti.Close();
                if (etk > 0)
                {
                    MessageBox.Show("Kayıt Silinmiştir");
                    UrunListesi();
                }
                else
                {
                    MessageBox.Show("Kayıt silinirken hata oluştu");
                }
            }
        }
    }
}
