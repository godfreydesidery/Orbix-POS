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
    public partial class SetupForm : Form
    {
        public SetupForm()
        {
            InitializeComponent();
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            txtAddress.Text = "";
            txtAddress.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(txtAddress.Text == "")
            {
                MessageBox.Show("Please enter server address");
            }else
            {
                //Continue processing
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
