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
    public partial class FormPettyCash : Form
    {
        public FormPettyCash()
        {
            InitializeComponent();
        }
        
        private void button28_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            global::System.String amount = txtAmount.Text;
            double test;
            if ((double.TryParse(amount, out test)) & ((Convert.ToDouble(amount)) >= (0d)))
            {
                btnOK.Enabled = true;
            }
            else
            {
                txtAmount.Text = "";
                btnOK.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtAmount.Text = "";
            txtDescription.Text = "";
        }

        private double getCurrentCash()
        {
            double available = 0d;
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

        private object getCurrentFloat()
        {
            double available = 0d;
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
            available = till_.floatBalance;
            return available;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string amount = txtAmount.Text;
            string detail = txtDescription.Text;
            if (getCurrentCash() < Convert.ToDouble(amount))
            {
                MessageBox.Show("Could not complete operation. Insufficient cash amount available.", "Error: Insufficient Funds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult res = MessageBox.Show("Petty Cash amount: " + LCurrency.displayValue(txtAmount.Text) + " Confirm?", "Confirm Petty Cash", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                // record petty cash

                var pettyCash = new PettyCash();
                pettyCash.amount = Convert.ToDouble(amount);
                pettyCash.details = txtDescription.Text;
                pettyCash.till.no = Till.TILLNO;
                var response = new object();
                var json = new JObject();
                try
                {
                    response = Web.post(pettyCash, "petty_cashs/collect_by_till_no?no=" + Till.TILLNO);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }

                MessageBox.Show("Petty cash registered successifully");
                this.Dispose();
            }
        }
    }
}
