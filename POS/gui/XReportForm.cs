using POS.general;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class XReportForm : Form
    {
        double totalAmount = 0d;
        public XReportForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void XReportForm_Load(object sender, EventArgs e)
        {
            string tillno = Till.TILLNO;
            string currentdate = general.Day.bussinessDate;
            string query = "SELECT SUM(amount) FROM `sale` WHERE `till_no`='" + tillno + "' AND `date`='" + currentdate + "' ";
            var command = new MySqlCommand();
            var conn = new MySqlConnection(Database.conString);
            try
            {
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                try
                {
                    totalAmount = command.ExecuteScalar;
                }
                catch (Exception ex)
                {
                    totalAmount = 0;
                }

                txtAmount.Text = (string) LCurrency.displayValue(totalAmount.ToString());
            }
            catch (Devart.Data.MySql.MySqlException ex)
            {
                LError.databaseConnection();
                return;
            }
        }
    }
}
