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
    public partial class FloatForm : Form
    {
        double currentFloat = 0d;
        double addFloat = 0d;
        double newFloat = 0d;

        public FloatForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void FloatForm_Load(object sender, EventArgs e)
        {
            var response = new object();
            var json = new JObject();
            try
            {
                response = Web.get_("tills/get_float_by_no?no=" + Till.TILLNO);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            json = JObject.Parse(response.ToString());
            Till till_ = JsonConvert.DeserializeObject<Till>(json.ToString());
            currentFloat = till_.floatBalance;
            txtCurrentFloat.Text =(string) LCurrency.displayValue(currentFloat.ToString());
        }

        private void txtAdd_Enter(object sender, EventArgs e)
        {
            txtDeduct.Text = "";
        }

        private void txtAdd_TextChanged(object sender, EventArgs e)
        {
            string amount = txtAdd.Text;
            double test;
            if (double.TryParse(amount, out test) & Convert.ToDouble(amount) >= 0)
            {
                newFloat = currentFloat + Convert.ToDouble(amount);
                txtNewFloat.Text =(string) LCurrency.displayValue(newFloat.ToString());
            }
            else
            {
                txtAdd.Text = "";
                txtNewFloat.Text = "";
            }
        }

        private void txtNewFloat_TextChanged(object sender, EventArgs e)
        {
            if (txtNewFloat.Text == "")
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult res = 0;
            if (Convert.ToDouble(txtAdd.Text) > 0)
            {
                txtDeduct.Text = "";
                res = MessageBox.Show("Increase amount: " + LCurrency.displayValue(txtAdd.Text) + " New amount: " + LCurrency.displayValue(txtNewFloat.Text) + " Confirm?", "Confirm float increase", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (Convert.ToDouble(txtDeduct.Text) > 0)
            {
                txtAdd.Text = "";
                res = MessageBox.Show("Deduct amount: " + LCurrency.displayValue(txtDeduct.Text) + " New amount: " + LCurrency.displayValue(txtNewFloat.Text) + " Confirm?", "Confirm float deduction", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (res == DialogResult.Yes)
            {
                currentFloat = (double)LCurrency.getValue(txtNewFloat.Text);
                txtCurrentFloat.Text = (string) LCurrency.displayValue(currentFloat.ToString());
                var till = new Till();
                till.no = Till.TILLNO;
                till.computerName = "NA";
                till.name = "NA";
                till.floatBalance = currentFloat;
                var response = new object();
                var json = new JObject();
                try
                {
                    response = Web.put(till, "tills/update_float_by_no?no=" + Till.TILLNO);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }

                txtAdd.Text = "";
                txtDeduct.Text = "";
                MessageBox.Show("Float updated successifully");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtAdd.Text = "";
        }

        private void txtDeduct_Enter(object sender, EventArgs e)
        {
            txtAdd.Text = "";
        }

        private void txtDeduct_TextChanged(object sender, EventArgs e)
        {
            string amount = txtDeduct.Text;
            double test;
            if (double.TryParse(amount, out test) & Convert.ToDouble(amount) >= 0d & Convert.ToDouble(amount) <= (double)currentFloat)
            {
                newFloat = currentFloat - Convert.ToDouble(amount);
                txtNewFloat.Text = (string) LCurrency.displayValue(newFloat.ToString());
            }
            else
            {
                txtDeduct.Text = "";
                txtNewFloat.Text = "";
            }
        }
    }
}
