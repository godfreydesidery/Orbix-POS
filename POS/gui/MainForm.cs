using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POS.general;
using POS.gui;
using POS.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace POS
{
    public partial class MainForm : Form
    {
        string barCode = "";
        string code = "";
        string ShortDescription = "";
        string description = "";
        double packSize = 1;
        double price = 0;
        double vat = 0;
        double discountRatio = 0;
        double qty = 0;
        double amount = 0;
        bool @void = false;
        bool allowVoid = false;
        int seq = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblAlias.Text = POS.general.Day.CURRENT_DATE + " Welcome " + User.ALIAS;

            
            if (User.authorize("SELLING"))
            {
                dtgrdProductList.Enabled = true;               
                btnPay.Enabled = true;
            }

            var product = new Product();
            // longList = item.getItemDescriptions()
        }

        private void button19_Click(object sender, EventArgs e)
        {
            refreshList();
            calculateValues();
            if (dtgrdProductList.RowCount > 0 & isAllVoid() == false)
            {
                FormPayPoint frmPayPoint = new FormPayPoint();
                FormPayPoint.total = (double) LCurrency.getValue(txtGrandTotal.Text); //String.Format("{0:0.00}", txtGrandTotal.Text);
                frmPayPoint.ShowDialog(this);
                if (frmPayPoint.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    calculateValues();
                }
                else
                {
                    var receipt = new Receipt();
                    receipt.cash = FormPayPoint.cash;
                    receipt.voucher = FormPayPoint.voucher;
                    receipt.deposit = FormPayPoint.deposit;
                    receipt.loyalty = FormPayPoint.loyalty;
                    receipt.crCard = FormPayPoint.CRCard;
                    receipt.cheque = FormPayPoint.cheque;
                    receipt.cap = FormPayPoint.CAP;
                    receipt.invoice = FormPayPoint.invoice;
                    receipt.crNote = FormPayPoint.CRNote;
                    receipt.mobile = FormPayPoint.mobile;
                    receipt.other = FormPayPoint.other;
                    string cashReceived = FormPayPoint.cashReceived;
                    string balance = FormPayPoint.balance;
                    receipt.no = "NA";
                    receipt.till.no = Till.TILLNO;
                  //  receipt.issueDate = general.Day.bussinessDate;
                    receipt.cart.id = txtId.Text;
                    var response = new object();
                    var json = new JObject();
                    response = Web.post(receipt, "receipts/new");
                    var receiptToPrint = JsonConvert.DeserializeObject<Receipt>(response.ToString());
                    string tillNo = receiptToPrint.till.no;
                    string date_ = general.Day.bussinessDate;
                    string receiptNo = receiptToPrint.no;
                    string TIN = Company.TIN.ToString();
                    string VRN = Company.VRN.ToString();
                    if (printReceipt(tillNo, receiptNo, date_, TIN, VRN, cashReceived, balance) == true)
                    {
                    }
                    // '
                    else
                    {
                        MessageBox.Show("Payment canceled", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    loadCart(txtId.Text, Till.TILLNO);
                    calculateValues();
                    allowVoid = false;
                    if (dtgrdProductList.RowCount == 1)
                    {
                        txtId.Text = "";
                    }
                }
            }          
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

        protected virtual object place(string key)
        {
            try
            {
                int row = dtgrdProductList.CurrentCell.RowIndex;
                int col = dtgrdProductList.CurrentCell.ColumnIndex;
                if (dtgrdProductList.CurrentCell.ColumnIndex == 0 | dtgrdProductList.CurrentCell.ColumnIndex == 1 | dtgrdProductList.CurrentCell.ColumnIndex == 2 | dtgrdProductList.CurrentCell.ColumnIndex == 7)
                {
                    dtgrdProductList.Select();
                    dtgrdProductList.CurrentCell = dtgrdProductList[col, row];
                    TextBox control = (TextBox)dtgrdProductList.EditingControl;
                    control.SelectionStart = dtgrdProductList.CurrentCell.Value.ToString().Length;
                    control.SelectionLength = 0;
                    SendKeys.Send(key);
                }
                else
                {
                    dtgrdProductList.Select();
                    dtgrdProductList.CurrentCell = dtgrdProductList[col, row];
                    SendKeys.Send(key);
                }
            }
            catch (Exception ex)
            {
                // MsgBox(ex.StackTrace)
                try
                {
                    SendKeys.Send(key);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.StackTrace);
                }
            }

            return null;
        }

        private void dtgrdProductList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double qty = 0d;
            string sn = "";
            try
            {
                qty = (double)dtgrdProductList[7, e.RowIndex].Value;
                sn = (string)dtgrdProductList[11, e.RowIndex].Value;
            }
            catch (Exception ex)
            {
            }
            if ((double)dtgrdProductList[7, e.RowIndex].Value >= 0 & (double)dtgrdProductList[7, e.RowIndex].Value <= 1000 & (string)dtgrdProductList[11, e.RowIndex].Value != "")
            {
                if (qty > 0)
                {
                    updateQty(qty, sn);
                }
                else
                {
                    updateQty(0, sn);
                }

                calculateValues();
            }
            else if ((double)dtgrdProductList[7, e.RowIndex].Value <= 0 & (string)dtgrdProductList[1, e.RowIndex].Value != "")
            {
                MessageBox.Show("Invalid quantity value. Quantity value should be between 1 and 1000", "Error: Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtgrdProductList[7, e.RowIndex].Value = 1;
                calculateValues();
            }

            try
            {
                if (dtgrdProductList.CurrentCell.ColumnIndex == 2)
                {
                    // search() 'do further research
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                dtgrdProductList.EndEdit();
                search();
                return true;
            }
            // Return MyBase.ProcessCmdKey(msg, keyData)
            return false;
        }

        private void search()
        {
            try
            {
                int row = dtgrdProductList.CurrentCell.RowIndex;
                int col = dtgrdProductList.CurrentCell.ColumnIndex;
                if (col == 0 & row == dtgrdProductList.RowCount - 2)
                {
                    // search item
                    // add item to list
                    string value = "";
                    bool search = true;
                    try
                    {
                        value = dtgrdProductList[col, row].Value.ToString();
                        if (string.IsNullOrEmpty(value) | value == "0")
                        {
                            search = false;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        search = false;
                    }

                    if (search == true)
                    {
                        bool found = searchByBarcode(value, 1);
                        if (found == true)
                        {
                            // item found
                            if (SaleSequence.multiple == true)
                            {
                                for (int i = 0; i <= 7; i++)
                                {
                                    // SendKeys.Send("{right}")
                                }
                            }
                            else
                            {
                                // SendKeys.Send("{down}")
                            }
                        }
                        else if (found == false)
                        {
                            MessageBox.Show("Product not found", "Error: Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (col == 7 & row == dtgrdProductList.RowCount - 2)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        // SendKeys.Send("{left}")
                    }
                    // SendKeys.Send("{down}")
                }

                if (col == 1 & row == dtgrdProductList.RowCount - 2)
                {
                    // search item
                    // add item to list
                    string value = "";
                    bool search = true;
                    try
                    {
                        value = dtgrdProductList[col, row].Value.ToString();
                        if (string.IsNullOrEmpty(value) | value == "0")
                        {
                            search = false;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        search = false;
                    }

                    if (search == true)
                    {
                        bool found = searchByCode(value, 1);
                        if (found == true)
                        {
                            // item found
                            if (SaleSequence.multiple == true)
                            {
                                for (int i = 0; i <= 6; i++)
                                {
                                    // SendKeys.Send("{right}")
                                }
                            }
                            else
                            {
                                // SendKeys.Send("{down}")
                                // SendKeys.Send("{left}")
                            }
                        }
                        else if (found == false)
                        {
                            MessageBox.Show("Product not found", "Error: Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (col == 7 & row == dtgrdProductList.RowCount - 2)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        // SendKeys.Send("{left}")
                    }
                    // SendKeys.Send("{down}")
                }

                if (col == 2 & row == dtgrdProductList.RowCount - 2)
                {
                    // search item
                    // add item to list
                    string value = "";
                    bool search = true;
                    try
                    {
                        value = dtgrdProductList[col, row].Value.ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            search = false;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.StackTrace);
                        search = false;
                    }

                    if (search == true)
                    {
                        bool found = searchByDescription(value, 1);
                        if (found == true)
                        {
                            // item found
                            if (SaleSequence.multiple == true)
                            {
                                for (int i = 0; i <= 4; i++)
                                {
                                    // SendKeys.Send("{right}")
                                }
                            }
                            else
                            {
                                // SendKeys.Send("{down}")
                                // SendKeys.Send("{left}")
                                // SendKeys.Send("{left}")
                            }
                        }
                        else if (found == false)
                        {
                            MessageBox.Show("Product not found", "Error: Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (col == 7 & row == dtgrdProductList.RowCount - 2)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        // SendKeys.Send("{left}")
                    }
                    // SendKeys.Send("{down}")
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // refreshList()
            // dtgrdProductList.Select()
        }

        private bool searchByBarcode(string barcode, int q)
        {
            return search(barcode, "", "", q);
        }

        private bool searchByCode(string code, int q)
        {
            return search("", code, "", q);
        }

        private bool searchByDescription(string description, int q)
        {
            return search("", "", description, q);
        }

        private bool search(string barcode, string code, string description, double q)
        {
            bool found = false;
            var response = new object();
            var json = new JObject();
            int no = 0;
            int row = 0;
            try
            {
                if (!string.IsNullOrEmpty(barcode))
                {
                    response = Web.get_("products/get_by_barcode?barcode=" + barcode);
                }
                else if (!string.IsNullOrEmpty(code))
                {
                    response = Web.get_("products/get_by_code?code=" + code);
                }
                else
                {
                    response = Web.get_("products/get_by_description?description=" + description);
                }

                json = JObject.Parse(response.ToString());
                Product product = JsonConvert.DeserializeObject<Product>(json.ToString());
                barcode = product.primaryBarcode;
                code = product.code;
                description = product.description;
                ShortDescription = product.shortDescription;
                packSize = product.packSize;
                discountRatio = product.discountRatio;
                vat = product.vat;
                qty = q;
                price = product.sellingPriceVatIncl;
                amount = (double) qty * price * (1 - discountRatio / 100);
                found = true;
                if (string.IsNullOrEmpty(barcode))
                {
                    found = false;
                    loadCart(txtId.Text, Till.TILLNO);
                }

                try
                {
                    row = dtgrdProductList.CurrentCell.RowIndex;
                }
                catch (Exception ex)
                {
                    dtgrdProductList.Rows.Add();
                    row = dtgrdProductList.CurrentCell.RowIndex;
                }

                if (found == true)
                {
                    dtgrdProductList[0, row].Value = barcode;
                    dtgrdProductList[1, row].Value = code;
                    dtgrdProductList[2, row].Value = description;
                    dtgrdProductList[4, row].Value = LCurrency.displayValue(price.ToString());
                    dtgrdProductList[5, row].Value = LCurrency.displayValue(vat.ToString());
                    dtgrdProductList[6, row].Value = LCurrency.displayValue(discountRatio.ToString());
                    dtgrdProductList[7, row].Value = qty;
                    dtgrdProductList[8, row].Value = LCurrency.displayValue(amount.ToString());
                    dtgrdProductList[10, row].Value = description;
                    dtgrdProductList[0, row].ReadOnly = true;
                    dtgrdProductList[1, row].ReadOnly = true;
                    dtgrdProductList[2, row].ReadOnly = true;
                    seq = seq + 1;
                    AddToCart("", Till.TILLNO, dtgrdProductList[0, row].Value.ToString(), dtgrdProductList[1, row].Value.ToString(), dtgrdProductList[2, row].Value.ToString(),(double)dtgrdProductList[4, row].Value, (double)dtgrdProductList[5, row].Value, (double)dtgrdProductList[6, row].Value, (double)dtgrdProductList[7, row].Value, (double)dtgrdProductList[8, row].Value, dtgrdProductList[10, row].Value.ToString());
                    if (dtgrdProductList.RowCount > 1)
                    {
                        if ((int)dtgrdProductList[7, row - 1].Value > 1)
                        {
                            SaleSequence.multiple = true;
                        }
                        else
                        {
                            SaleSequence.multiple = false;
                        }
                    }
                }
                else
                {
                    loadCart(txtId.Text, Till.TILLNO);
                }
            }
            catch (Exception ex)
            {
                dtgrdProductList[0, row].Value = "";
                dtgrdProductList[1, row].Value = "";
                dtgrdProductList[2, row].Value = "";
                dtgrdProductList[4, row].Value = "";
                dtgrdProductList[5, row].Value = "";
                dtgrdProductList[6, row].Value = "";
                dtgrdProductList[7, row].Value = "";
                dtgrdProductList[8, row].Value = "";
                dtgrdProductList.EndEdit();
                refreshList();
                calculateValues();
            }

            dtgrdProductList.EndEdit();
            refreshList();
            calculateValues();
            loadCart(txtId.Text, Till.TILLNO);

            return found;
        }

        private object refreshList()
        {
            if (dtgrdProductList.RowCount > 0)
            {
                int max = dtgrdProductList.RowCount - 2;
                for (int i = max; i >= 0; i -= 1)
                {
                    if ((string)dtgrdProductList[1, i].Value == "" | (double)dtgrdProductList[7, i].Value <= 0)
                    {
                        // dtgrdProductList.Rows.RemoveAt(i)
                    }
                }
            }

            try
            {
                dtgrdProductList.EndEdit();
                if (voidRow > -1)
                {
                    dtgrdProductList[9, voidRow].Value = false;
                    if (dtgrdProductList[9, voidRow].Value == true)
                    {
                        dtgrdProductList[9, voidRow].Value = false;
                    }
                    else
                    {
                        dtgrdProductList[9, voidRow].Value = true;
                    }
                }
                voidRow = -1;
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message)
            }

            return null;
        }

        private bool allow = false;

        private object calculateValues()
        {
            try
            {
                dtgrdProductList.EndEdit();
                double _total = 0;
                double _vat = 0;
                double _discount = 0;
                double _grandTotal = 0;
                int rows = dtgrdProductList.RowCount;
                for (int i = 0, loopTo = rows - 2; i <= loopTo; i++)
                {
                    double price = (double)LCurrency.getValue(dtgrdProductList[4, i].Value.ToString());
                    double vat = (double)LCurrency.getValue(dtgrdProductList[5, i].Value.ToString());
                    double discountRatio = (double)LCurrency.getValue(dtgrdProductList[6, i].Value.ToString());
                    double qty = (double)dtgrdProductList[7, i].Value;
                    double amount = price * qty * (1d - discountRatio / 100d);
                    dtgrdProductList[8, i].Value = LCurrency.displayValue(amount.ToString());
                    if ((bool)dtgrdProductList[9, i].Value == false)
                    {
                        _total = _total + (double)LCurrency.getValue(dtgrdProductList[8, i].Value.ToString());
                        _vat = _vat + (double)(LCurrency.getValue(dtgrdProductList[5, i].Value.ToString())) * (double)LCurrency.getValue(dtgrdProductList[8, i].Value.ToString()) / (100 + (double)(LCurrency.getValue(dtgrdProductList[5, i].Value.ToString)));
                        double discountedPrice = (double)LCurrency.getValue(price.ToString()) * (1 - (double)discountRatio / 100d);
                        _discount = _discount + (price - discountedPrice) * qty;

                        // _discount = _discount + (Val(LCurrency.getValue(dtgrdProductList.Item(6, i).Value.ToString)) / 100) * Val(LCurrency.getValue(dtgrdProductList.Item(8, i).Value.ToString))
                    }
                }

                _grandTotal = _total;
                txtTotal.Text = (string) LCurrency.displayValue(_total.ToString());
                txtDiscount.Text = (string) LCurrency.displayValue(_discount.ToString());
                txtTax.Text = (string) LCurrency.displayValue(_vat.ToString());
                txtGrandTotal.Text = (string) LCurrency.displayValue(_grandTotal.ToString());
            }
            catch (Exception ex)
            {
                // MsgBox(ex.StackTrace)
            }

            return null;
        }

        private void dtgrdProductList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = -1;
            int col = -1;
            try
            {
                row = dtgrdProductList.CurrentRow.Index;
                col = dtgrdProductList.CurrentCell.ColumnIndex;
            }
            catch (Exception ex)
            {
                row = -1;
            }
            dtgrdProductList.EndEdit();
            if (dtgrdProductList.CurrentCell.ColumnIndex == 9)
            {
                string sn = dtgrdProductList[11, dtgrdProductList.CurrentCell.RowIndex].Value.ToString();
                loadCart(txtId.Text, Till.TILLNO);
                bool @void = checkVoid(Till.TILLNO, sn);
                if (allowVoid == false)
                {
                    if (User.authorize("VOID"))
                    {
                        allowVoid = true;
                    }
                    else
                    {
                        FormAllow frmAllow = new FormAllow();
                        frmAllow.ShowDialog();
                        if (FormAllow.allowed == true)
                        {
                            allowVoid = true;
                        }
                    }
                }

                if (allowVoid == true)
                {
                    if (@void == true)
                    {
                        unvoid(Till.TILLNO, sn);
                    }
                    else
                    {
                        _void(Till.TILLNO, sn);
                    }
                }

                loadCart(txtId.Text, Till.TILLNO);
            }
            refreshList();
            calculateValues();

        }
        FormDiscount discountDialog;

        private void dtgrdProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = -1;
            int col = -1;
            double amount = 0;
            string item = "";
            string unitPrice = "";
            try
            {
                row = dtgrdProductList.CurrentRow.Index;
                col = dtgrdProductList.CurrentCell.ColumnIndex;
                amount = (double)LCurrency.getValue(dtgrdProductList[4, row].Value.ToString());
                item = "(" + dtgrdProductList[1, row].Value.ToString() + ")  " + dtgrdProductList[2, row].Value.ToString();
                unitPrice = dtgrdProductList[4, row].Value.ToString();
            }
            catch (Exception ex)
            {
                row = -1;
            }
            dtgrdProductList.EndEdit();


            if (dtgrdProductList.CurrentCell.ColumnIndex == 6)
            {
                string sn = dtgrdProductList.Item(11, dtgrdProductList.CurrentCell.RowIndex).Value;
                loadCart(txtId.Text, Till.TILLNO);

                // process discount

                if (User.authorize("DISCOUNT") == true)
                {

                    discountDialog = new FormDiscount();
                    discountDialog.lblItem.Text = item;
                    discountDialog.lblUnitPrice.Text = "Unit Price " + unitPrice;
                    discountDialog.ShowDialog();
                    if (discountDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        double discount = (double)discountDialog.txtDiscount.Text;
                        if ((discount) <= amount)
                        {
                            var discPercentage = (discount) / amount * 100;
                            updateDiscount(Till.TILLNO, sn, discPercentage);
                        }
                        else
                        {
                            MessageBox.Show("Invalid Discount Amount. Discount should be less than Unit Price", "Error: Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        discountDialog.Dispose();
                    }
                    else
                    {
                        discountDialog.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("Operation denied!", "Error: Operation denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                loadCart(txtId.Text, Till.TILLNO);
            }
            refreshList();
            calculateValues();
        }

        private void updateDiscount(string tillNo, string sn, double discRatio)
        {
            var detail = new CartDetail();
            detail.id = sn;
            detail.discountRatio = discRatio;
            Web.put(detail, "carts/update_discount?detail_id=" + sn + "&discount_ratio=" + discRatio.ToString());
            // ''''''''''
        }
        private int RECEIPT_NO0 = 0;
        private double totalTaxReturns = 0d;

        private bool isAllVoid()
        {
            bool allVoid = true;
            for (int i = 0, loopTo = dtgrdProductList.RowCount - 2; i <= loopTo; i++)
            {
                if ((bool) dtgrdProductList[9, i].Value == false)
                {
                    allVoid = false;
                }
            }

            return allVoid;
        }

        private bool printReceipt(string tillNo, string receiptNo, string date_, string TIN, string VRN, string cash, string balance)
        {
            bool printed = false;
            int size = -1;
            for (int i = 0, loopTo = dtgrdProductList.RowCount - 2; i <= loopTo; i++)
            {
                if ((bool) dtgrdProductList[9, i].Value == false)
                {
                    size = size + 1;
                }
            }

            var code = new string[size + 1];
            var descr = new string[size + 1];
            var qty = new string[size + 1];
            var price = new string[size + 1];
            var tax = new string[size + 1];
            var amount = new string[size + 1];
            string subTotal = txtTotal.Text;
            string totalVat = txtTax.Text;
            string total = txtGrandTotal.Text;
            string discountRatio = txtDiscount.Text;
            int count = 0;
            for (int i = 0, loopTo1 = dtgrdProductList.RowCount - 2; i <= loopTo1; i++)
            {
                if ((bool) dtgrdProductList[9, i].Value == false)
                {
                    code[count] = dtgrdProductList[1, i].Value.ToString();
                    descr[count] = new Product().getShortDescription(code[count]);
                    qty[count] =dtgrdProductList[7, i].Value.ToString();
                    price[count] = dtgrdProductList[4, i].Value.ToString();
                    tax[count] = dtgrdProductList[5, i].Value.ToString();
                    amount[count] = dtgrdProductList[8, i].Value.ToString();
                    count = count + 1;
                }
            }

            if ((bool)PointOfSale.printReceipt(tillNo, receiptNo, date_, TIN, VRN, code, descr, qty, price, tax, amount, subTotal, totalVat, total, cash, balance) == true)
            {
                printed = true;
            }

            return printed;
        }
        string saleId = "";


        private bool updateQty(double qty, string sn)
        {
            var detail = new CartDetail();
            detail.id = sn;
            detail.qty = qty;
            var response = new object();
            var json = new JObject();
            response = Web.put(detail, "carts/update_qty?detail_id=" + sn + "&qty=" + qty.ToString());
            if ((bool) response == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            RECEIPT_NO0 = new Receipt().makeReceipt(Till.TILLNO, general.Day.bussinessDate);
            loadCart(txtId.Text, Till.TILLNO);
        }

        private void dtgrdProductList_CellClick2(object sender, DataGridViewCellEventArgs e)
        {
            {
                try
                {
                    if (dtgrdProductList.CurrentCell.ColumnIndex == 2)
                    {
                        var control = new TextBox();
                        control = (TextBox)dtgrdProductList.EditingControl;
                        var list = new List<string>();
                        var mySource = new AutoCompleteStringCollection();
                        var product_ = new Product();
                        list = product_.getDescriptions();
                        mySource.AddRange(list.ToArray());
                        control.AutoCompleteCustomSource = mySource;
                        control.AutoCompleteMode = AutoCompleteMode.Suggest;
                        control.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }
            }

        }
        private List<string> longList = new List<string>();
        private List<string> shortList = new List<string>();
        
        [DllImport("kernel32")]
        static extern bool Wow64DisableWow64FsRedirection(ref long oldvalue);
        [DllImport("kernel32")]
        static extern bool Wow64EnableWow64FsRedirection(ref long oldvalue);
        private string osk = @"C:\Windows\System32\osk.exe";


        private void startOSK()
        {
            long old;
            if (Environment.Is64BitOperatingSystem)
            {
                if (Wow64DisableWow64FsRedirection(old))
                {
                    Process.Start(osk);
                    Wow64EnableWow64FsRedirection(old);
                }
            }
            else
                Process.Start(osk);
        }


        private void AddToCart(string sn, string tillNo, string barcode, string code, string description, double sellingPriceVatIncl, double vat, double discountRatio, double qty, double amount, string shortDescr)
        {
            var cart = new Cart();
            cart.id = txtId.Text;
            cart.till.no = tillNo;
            var cartDetail = new CartDetail();
            cartDetail.id = sn;
            cartDetail.barcode = barcode;
            cartDetail.code = code;
            cartDetail.description = description;
            cartDetail.sellingPriceVatIncl = sellingPriceVatIncl;
            cartDetail.vat = vat;
            cartDetail.discountRatio = discountRatio;
            cartDetail.qty = qty;
            cart.cartDetails.Add(cartDetail);
            var response = new object();
            var json = new JObject();
            if (txtId.Text == "")
            {
                response = Web.post(cart, "carts/new");
            }
            else
            {
                response = Web.put(cart, "carts/update_by_id?id=" + txtId.Text);
            }

            json = JObject.Parse(response.ToString());
            if (txtId.Text == "")
            {
                txtId.Text = json.SelectToken("id").ToString();
            }
        }

        private bool checkVoid(string tillNo, string sn)
        {
            var response = new object();
            var json = new JObject();
            response = Web.get_("carts/check_voided?detail_id=" + sn);
            if ((bool) response == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void _void(string tillNo, string id)
        {
            var cartDetail = new CartDetail();
            cartDetail.id = id;
            cartDetail.cart.id = txtId.Text;
            var response = new object();
            var json = new JObject();
            response = Web.put(cartDetail, "carts/void?detail_id=" + id);
            loadCart(txtId.Text, Till.TILLNO);
        }

        private void unvoid(string tillNo, string id)
        {
            var cartDetail = new CartDetail();
            cartDetail.id = id;
            cartDetail.cart.id = txtId.Text;
            var response = new object();
            var json = new JObject();
            response = Web.put(cartDetail, "carts/unvoid?detail_id=" + id);
            loadCart(txtId.Text, Till.TILLNO);
        }

        private void loadCart(string id, string tillNo)
        {
            dtgrdProductList.Rows.Clear();
            var response = new object();
            var json = new JObject();
            try
            {
                response = Web.get_("carts/get_by_id_and_till_no?id=" + id + "&till_no=" + tillNo);
                json = JObject.Parse(response.ToString());
            }
            catch (Exception ex)
            {
                return;
            }

            Cart cart = JsonConvert.DeserializeObject<Cart>(json.ToString());
            if (cart.cartDetails == null)
            {
                int i = 0;
                foreach (CartDetail detail in cart.cartDetails)
                {
                    var dtgrdRow = new DataGridViewRow();
                    DataGridViewCell dtgrdCell;
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = detail.barcode;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = detail.code;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = detail.description;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = "";
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = LCurrency.displayValue(detail.sellingPriceVatIncl.ToString());
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = detail.vat;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = detail.discountRatio;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = detail.qty;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = amount;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewCheckBoxCell();
                    if (detail.voided == 1)
                    {
                        dtgrdCell.Value = true;
                    }
                    else
                    {
                        dtgrdCell.Value = false;
                    }

                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = "";
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdCell = new DataGridViewTextBoxCell();
                    dtgrdCell.Value = detail.id;
                    dtgrdRow.Cells.Add(dtgrdCell);
                    dtgrdProductList.Rows.Add(dtgrdRow);
                    dtgrdProductList[0, i].ReadOnly = true;
                    dtgrdProductList[1, i].ReadOnly = true;
                    dtgrdProductList[2, i].ReadOnly = true;
                    i = i + 1;
                }

                refreshList();
                calculateValues();
                dtgrdProductList.CurrentCell = dtgrdProductList[0, dtgrdProductList.RowCount - 1];
            }
        }
    }
}
