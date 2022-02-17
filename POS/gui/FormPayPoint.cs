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
    public partial class FormPayPoint : Form
    {
        public static double cash = 0d;
        public static double voucher = 0d;
        public static double deposit = 0d;
        public static double loyalty = 0d;
        public static double CRCard = 0d;
        public static double CAP = 0d;
        public static double invoice = 0d;
        public static double CRNote = 0d;
        public static double mobile = 0d;
        public static double cheque = 0d;
        public static double other = 0d;

        public static double total = 0d;
        public static string cashReceived = "";
        public static string balance = "";

        string MODE = "";
        public FormPayPoint()
        {
            InitializeComponent();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double amount = double.Parse(txtTotal.Text);
            if (double.Parse(txtBalance.Text) >= 0)
            {
                // Dim confirm As Integer = MessageBox.Show("Total amount payable is " + LCurrency.displayValue(amount.ToString) + ". Confirm payment?", "Confirm payment", MessageBoxButtons.YesNo)
                DialogResult confirm = MessageBox.Show("Total amount payable is " + LCurrency.displayValue(amount.ToString()) + ". Confirm payment?", "Confirm Payment: " + LCurrency.displayValue(amount.ToString()), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    cashReceived = (string) LCurrency.displayValue(txtAmountReceived.Text);
                    balance = (string) LCurrency.displayValue(txtBalance.Text);
                    // commit payment
                    cash = double.Parse(txtCash.Text) - (double)(LCurrency.getValue(txtBalance.Text));
                    voucher = double.Parse(txtVoucher.Text);
                    deposit = double.Parse(txtDeposit.Text);
                    loyalty = double.Parse(txtLoyalty.Text);
                    CRCard = double.Parse(txtCRCard.Text);
                    CAP = double.Parse(txtCAP.Text);
                    invoice = double.Parse(txtInvoice.Text);
                    CRNote = double.Parse(txtCRNote.Text);
                    mobile = double.Parse(txtMobile.Text);
                    cheque = double.Parse(txtCheque.Text);
                    Payment.setPayment(cash, voucher, deposit, loyalty, CRCard, CAP, invoice, CRNote, mobile);

                    // till register
                    // Till.tillTotalRegister(Till.TILLNO, cash, voucher, cheque, deposit, loyalty, CRCard, CAP, invoice, CRNote, mobile)
                    this.Dispose();
                }
                else
                {
                    // dont commit payment
                }
            }
            else
            {
                MessageBox.Show("Could not process payment. Insufficient funds", "Error: Insufficient funds", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static object updateTill()
        {
            SaleSequence.seqNo = SaleSequence.seqNo + 1;
            Till.tillTotalRegister(Till.TILLNO, cash, voucher, cheque, deposit, loyalty, CRCard, CAP, invoice, CRNote, mobile);
            return null;
        }

        private object recordSale(string saleId)
        {
            // sql for recording sale with the specified id

            return null;
        }

        private object calculateTotal()
        {
            double totalAmount = double.Parse(txtTotal.Text.Replace(",",""));
            double totalReceived = 0d;
            double balance;
            double cash;
            double voucher;
            double cheque;
            double deposit;
            double loyalty;
            double CRCard;
            double CAP;
            double invoice;
            double CRNote;
            double mobile;
            cash = double.Parse(txtCash.Text);
            voucher = double.Parse(txtVoucher.Text);
            cheque = double.Parse(txtCheque.Text);
            deposit = double.Parse(txtDeposit.Text);
            loyalty = double.Parse(txtLoyalty.Text);
            CRCard = double.Parse(txtCRCard.Text);
            CAP = double.Parse(txtCAP.Text);
            invoice = double.Parse(txtInvoice.Text);
            CRNote = double.Parse(txtCRNote.Text);
            mobile = double.Parse(txtMobile.Text);
            totalReceived = cash + voucher + cheque + deposit + loyalty + CRCard + CAP + invoice + CRNote + mobile;
            txtAmountReceived.Text =(string) LCurrency.displayValue(totalReceived.ToString());
            balance = totalReceived - totalAmount;
            txtBalance.Text = (string)LCurrency.displayValue(balance.ToString());
            return null;
        }

        private void txtCash_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtCash_KeyUp(object sender, KeyEventArgs e)
        {
            // use the plus equal key
            if (e.KeyCode == Keys.Oemplus)
            {
                clearAll();
                txtCash.Text =(string) LCurrency.getValue(txtTotal.Text);
            }
        }

        private void txtCash_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clearAll();
            txtCash.Text = (string)LCurrency.getValue(txtTotal.Text);
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            if ((bool) validateInput(txtCash.Text) == false)
            {
                txtCash.Text = "";
            }

            calculateTotal();
        }

        private object clearAll()
        {
            // clears all the fields
            txtCash.Text = "";
            txtVoucher.Text = "";
            txtDeposit.Text = "";
            txtLoyalty.Text = "";
            txtCRCard.Text = "";
            txtCAP.Text = "";
            txtInvoice.Text = "";
            txtCRNote.Text = "";
            txtMobile.Text = "";
            return null;
        }

        private object validateInput(string input)
        {
            double number = 0d;
            bool valid = false;
            if (double.TryParse(input, out number))
            {
                // parsedNumber is a valid number!
                valid = true;
            }
            
            return valid;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void txtBalance_TextChanged(object sender, EventArgs e)
        {
            if (double.Parse(txtBalance.Text.Replace(",","")) < 0)
            {
                txtValidBalance.Text = "Invalid Balance Value!";
            }
            else
            {
                txtValidBalance.Text = "";
            }
        }

        private void FormPayPoint_Load(object sender, EventArgs e)
        {
            {
                clearAll();
                txtTotal.Text = String.Format("{0:0.00}", total);
                RandomKeyGenerator KeyGen;
                int NumKeys;
                int i_Keys;
                string RandomKey = "";

                // MODIFY THIS TO GET MORE KEYS    - LAITH - 27/07/2005 22:48:30 -
                NumKeys = 6;
                KeyGen = new RandomKeyGenerator();
                KeyGen.KeyLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                KeyGen.KeyNumbers = "0123456789";
                KeyGen.KeyChars = 6;
                var loopTo = NumKeys;
                for (i_Keys = 1; i_Keys <= loopTo; i_Keys++)
                    RandomKey = KeyGen.Generate();
                calculateTotal();
                txtCash.Focus();
            }

        }

        protected virtual object place(string key)
        {
            try
            {
                if (MODE == "CASH")
                {
                    txtCash.Focus();
                    txtCash.SelectionStart = txtCash.Text.Length;
                    txtCash.SelectionLength = 0;
                    SendKeys.Send(key);
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            place("1");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            place("2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            place("3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            place("4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            place("5");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            place("6");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            place("7");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            place("8");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            place("9");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            place("0");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            place(".");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            place("00");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            place("{BACKSPACE}");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            place("{BACKSPACE}");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            place("{UP}");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            place("{DOWN}");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            place("{LEFT}");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            place("{RIGHT}");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
