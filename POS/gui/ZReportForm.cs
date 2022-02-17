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
    public partial class ZReportForm : Form
    {
        

        public ZReportForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void ZReportForm_Load(object sender, EventArgs e)
        {
            double cash = 0d;
            double CRCards = 0d;
            double giftVouchers = 0d;
            double cheque = 0d;
            double CRNotes = 0d;
            double newFloat = 0d;
            string tillno = Till.TILLNO;
            string currentDate = general.Day.bussinessDate;
            var command = new MySqlCommand();
            var conn = new MySqlConnection(Database.conString);
            MySqlDataReader reader;
            string query = "";

            try
            {
                conn.Open();
                query = "SELECT `sale`.`date`,`sale`.`id`,`sale`.`till_no`,`payment`.`sale_id`,`payment`.`cash`,`payment`.`voucher`,`payment`.`cr_card`,`payment`.`cr_note`,`payment`.`cheque` FROM `sale`,`payment` WHERE `sale`.`id`=`payment`.`sale_id` AND `sale`.`till_no`='" + Till.TILLNO + "' AND `sale`.`date`='" + currentDate + "'";
                Interaction.Command().Connection = conn;
                Interaction.Command().CommandType = CommandType.Text;
                Interaction.Command().CommandText = query;
                reader = Interaction.Command().ExecuteReader();
                while (reader.Read)
                {
                    cash = cash + Val(reader.GetString("cash"));
                    giftVouchers = giftVouchers + Val(reader.GetString("voucher"));
                    CRCards = CRCards + Val(reader.GetString("cr_card"));
                    CRNotes = CRNotes + Val(reader.GetString("cr_note"));
                    cheque = cheque + Val(reader.GetString("cheque"));
                }

                query = "SELECT `amount` FROM `float_balance` WHERE `till_no`='" + Till.TILLNO + "'";
                Interaction.Command().CommandText = query;
                reader = Interaction.Command().ExecuteReader();
                while (reader.Read)
                {
                    newFloat = Val(reader.GetString("amount"));
                    break;
                }

                conn.Close();
                txtCash.Text = LCurrency.displayValue(cash.ToString());
                txtCheques.Text = LCurrency.displayValue(cheque.ToString());
                txtGiftVouchers.Text = LCurrency.displayValue(giftVouchers.ToString());
                txtCRCards.Text = LCurrency.displayValue(CRCards.ToString());
                txtCRNotes.Text = LCurrency.displayValue(CRNotes.ToString());
                txtNewFloat.Text = LCurrency.displayValue(newFloat.ToString());
            }
            catch (Devart.Data.MySql.MySqlException ex)
            {
                LError.databaseConnection();
                return;
            }
        }
    }
}
