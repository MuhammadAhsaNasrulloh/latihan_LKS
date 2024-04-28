using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace latihan_LKS
{
    public partial class LogIn : Form
    {
        public static string koneksi = ("server=localhost;username=root;password=;database=db_lks");
        MySqlConnection conn = new MySqlConnection(koneksi);
        public LogIn()
        {
            InitializeComponent();
        }

        Function func = new Function();

        void clear()
        {
            txtUsername.Text = string.Empty;
            txtPw.Text = string.Empty;
        }

        private void cBPw_CheckedChanged(object sender, EventArgs e)
        {
            if(cBPw.Checked)
            {
                txtPw.PasswordChar = '\0';
            }
            else
            {
                txtPw.PasswordChar = '*';
            }
        }

        private int getUserId(String username)
        {
            string query = "SELECT id_user FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);

            int userId = -1;
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                object result = cmd.ExecuteScalar();

                if(result != null)
                {
                    userId = Convert.ToInt32(result);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(conn.State == ConnectionState.Open)conn.Close();
            }
            return userId;
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin();
            User user = new User();
            string username = txtUsername.Text;
            string password = txtPw.Text;

            if(func.isLogin(username, password))
            {
                DialogResult rslt = MessageBox.Show("Login Berhasil", "Succes!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if(rslt == DialogResult.OK)
                {
                    int userId = getUserId(username);
                    func.LogActivity(userId);

                    string tipeUser = func.getTipeUser(username);
                    switch (tipeUser)
                    {
                        case "admin":
                            admin.Show();
                            this.Hide();
                            break;
                        case "user":
                            user.Show();
                            this.Hide();
                            break;
                        default:
                            MessageBox.Show("Data tidak Valid");
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Gk ada lu bro");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
