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

namespace Win_AdoNetGiris
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection();//sql ile bağlantı kurmaya yarar
            baglanti.ConnectionString = @"Server = (localdb)\MSSQLLocalDB;Database = KuzeyYeli;Integrated Security = true";//windows authantication bağlantısı için
            //  baglanti.ConnectionString = "Server = localhost;Database = KuzeyYeli;User= sa;Pwd=123"; uzaktaki bir sql e vs bağlanmak için
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "select * from Urunler";
            komut.Connection = baglanti;//bu vertabanı bağlantısına git ve bir üst satırdakini çalıştı

            baglanti.Open();
            SqlDataReader rdr = komut.ExecuteReader();
            while (rdr.Read())
            {
                string adi = rdr["UrunAdi"].ToString();
                string fiyat = rdr["Fiyat"].ToString();
                string stok = rdr["Stok"].ToString();
                listUrunler.Items.Add(string.Format("{0}-{1}-{2}", adi, fiyat, stok));
            }
            baglanti.Close();
        }
    }
}
