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
    public partial class FormDiscount : Form
    {
        public string product = "";
        public string unitPrice = "";
        public double discount = 0;
        public FormDiscount()
        {
            InitializeComponent();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtDiscount.Text) <= 0)
            {
                txtDiscount.Text = "0";
            }
        }

        private void FormDiscount_Load(object sender, EventArgs e)
        {
            txtDiscount.Text = "0";
            lblProduct.Text = product;
            lblUnitPrice.Text = unitPrice;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            discount = Convert.ToDouble(txtDiscount.Text);
        }
    }
}
