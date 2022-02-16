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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblAlias.Text = POS.general.Day.CURRENT_DATE+" Welcome " + User.ALIAS;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            PayPointForm form = new PayPointForm();
            form.ShowDialog();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            CashPickUpForm form = new CashPickUpForm();
            form.ShowDialog();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            PettyCashForm form = new PettyCashForm();
            form.ShowDialog();
        }

        private void btnFloat_Click(object sender, EventArgs e)
        {
            FloatForm form = new FloatForm();
            form.ShowDialog();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            LockForm form = new LockForm();
            form.ShowDialog();
        }

        private void btnXReport_Click(object sender, EventArgs e)
        {
            XReportForm form = new XReportForm();
            form.ShowDialog();
        }

        private void btnZReport_Click(object sender, EventArgs e)
        {
            ZReportForm form = new ZReportForm();
            form.ShowDialog();
        }
    }
}
