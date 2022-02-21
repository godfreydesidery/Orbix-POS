using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class CashPickUpForm : Form
    {
        double currentAmount = 0;
        double pickUpAmount = 0;
        double newCashAmount = 0;
        double currentFloat = 0;

        public CashPickUpForm()
        {
            InitializeComponent();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        private void CashPickUpForm_Load(object sender, EventArgs e)
        {
            var response = new object();
            var json = new JObject();
            try
            {
                response = Web.get_(("tills/get_till_position_by_no?no=") + (Till.TILLNO));
            }
            catch (global::System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            json = JObject.Parse(response.ToString());
            general.Till till_ = JsonConvert.DeserializeObject<general.Till>(json.ToString());
            currentAmount = till_.cash;
            currentFloat = till_.floatBalance;
            txtAvailable.Text =(string) LCurrency.displayValue(currentAmount.ToString());
            txtAmount.Text = "";
            txtBalance.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtAmount.Text = "";
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            string amount = txtAmount.Text;
            double test;
            if (((double.TryParse(amount, out test)) & ((Convert.ToDouble(amount)) >= (0d)))) // And Val(amount) <= currentAmount Then
            {
                newCashAmount = ((currentAmount) - Convert.ToDouble(amount));
                txtBalance.Text =(string) LCurrency.displayValue(newCashAmount.ToString());
            }
            else
            {
                txtAmount.Text = "";
                txtBalance.Text = "";
            }
        }

        private double getCurrentCash()
        {
            double available = 0;
            var response = new object();
            var json = new JObject();
            try
            {
                response = Web.get_("tills/get_till_position_by_no?no=" + Till.TILLNO);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            json = JObject.Parse(response.ToString());
            Till till_ = JsonConvert.DeserializeObject<Till>(json.ToString());
            available = till_.cash;
            return available;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string amount = txtAmount.Text;
            // Dim detail As String = txtDetails.Text

            if (getCurrentCash() < Convert.ToDouble(amount))
            {
                MessageBox.Show("Could not complete operation. Insufficient cash amount available.", "Error: Insufficient Funds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult res = MessageBox.Show("Pick up amount: " + LCurrency.displayValue(txtAmount.Text) + " Confirm?", "Confirm Cash Pick up", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                // record petty cash

                var cashPickUp = new CashPickUp();
                cashPickUp.amount = Convert.ToDouble(amount);
                // pettyCash.details = txtDetails.Text
                cashPickUp.till.no = Till.TILLNO;
                var response = new object();
                var json = new JObject();
                try
                {
                    response = Web.post(cashPickUp, "cash_pick_ups/pick_up_by_till_no?no=" + Till.TILLNO);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }

                MessageBox.Show("Cash Pick up registered successifully");
                this.Dispose();
            }
        }

        private void txtBalance_TextChanged(object sender, EventArgs e)
        {
            if (txtBalance.Text == "")
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }
    }

}
