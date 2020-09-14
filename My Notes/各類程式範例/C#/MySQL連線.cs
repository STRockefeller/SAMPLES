using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.Common;

namespace _20191119
{
    public partial class frmBase : Form
    {
        public frmBase()
        {
            InitializeComponent();
        }

        private void frmBase_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dbHost = "C:\\Users\\user\\Desktop\\lalala";
            string dbUser = "admin_user1";
            string dbPass = "*****";
            string dbName = "mydatabase_atelierryza.sql";

            //use location
            //goes 1042 and didonot know why
            string cnnStr= "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            //use ip address
            string connString = "server=127.0.0.1;port=3306;user id=admin_user1;password=*****;database=mydatabase;charset=utf8;";

            MySql.Data.MySqlClient.MySqlConnection cnn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            try 
            {
                cnn.Open();
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("�L�k�s�u���Ʈw.");
                        break;
                    case 1045:
                        MessageBox.Show("�ϥΪ̱b���αK�X���~,�ЦA�դ@��.");
                        break;
                    default:
                        MessageBox.Show($"MySQL ERR Code:{ex.Number.ToString()}");
                        break;
                }
            }

            cnn.Close();

        }
    }
}
