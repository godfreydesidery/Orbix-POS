using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.gui
{
    public partial class FormAllow : Form
    {
        public static bool allowed = false;

        public FormAllow()
        {
            InitializeComponent();
        }

        private void FormAllow_Load(object sender, EventArgs e)
        {
            allowed = false;
            txtPassword.Focus();
        }

        private void checkAllowed(string uname, string pword)
        {
            bool allwd = false;
            // verify managers password
            string query = "SELECT `username`, `password`, `role`, `alias`, `status` FROM `users` WHERE `username`='" + uname + "' AND (`role`='MANAGER' OR `role`='CHIEF CASHIER')";
            try
            {
                MySqlCommand command = new MySqlCommand();
                MySqlConnection conn = new MySqlConnection(Database.conString);
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                MySqlDataReader reader = command.ExecuteReader();
                string username = "";
                string password = "";
                string status = "";
                while (reader.Read)
                {
                    username = reader.GetString("username");
                    password = reader.GetString("password");
                    status = reader.GetString("status");
                    MsgBox(reader.GetString("role"));
                }
                if (Hash.check(pword, password))
                    allwd = true;
            }
            catch (Devart.Data.MySql.MySqlException ex)
            {
                LError.databaseConnection();
                return Constants.vbNull;
                return;
            }
            return allwd;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            allowed = false;
            this.Dispose();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                txtUsername.Focus();
            }
        }
    }
}
