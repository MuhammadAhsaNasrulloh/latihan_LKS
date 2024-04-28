using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace latihan_LKS
{
    internal class Function
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;username=root;password=;database=db_lks;");

        public void command(String query)
        {
            try
            {
                if(conn.State == ConnectionState.Closed) conn.Open();      
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private bool isLoggedIn = false;

        public bool isLogin(string username, string password)
        {
            string query = "SELECT * FROM user WHERE username = @username AND password = @password";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if(dt.Rows.Count  > 0 )
            {
                isLoggedIn = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getTipeUser(string username)
        {
            string tipeUser = "";
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "SELECT tipe_user FROM user WHERE username = @username";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tipeUser = reader.GetString("tipe_user");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(conn.State == ConnectionState.Open)conn.Close();
            }
            return tipeUser;
        }

        public void LogActivity(int userId)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            string query = "INSERT INTO log(aktivitas, waktu, id_user) VALUES('Login', CURRENT_TIMESTAMP(), @iduser)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@iduser", userId);

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void LogAOutctivity(int userId)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            string query = "INSERT INTO log(aktivitas, waktu, id_user) VALUES('Log out', CURRENT_TIMESTAMP(), @iduser)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@iduser", userId);

            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
