using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POS.general;
using POS.gui;
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
                qty =(double) dtgrdProductList[7, e.RowIndex].Value;
                sn =(string) dtgrdProductList[11, e.RowIndex].Value;
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
            else if ((double)dtgrdProductList[7, e.RowIndex].Value <= 0 & (string) dtgrdProductList[1, e.RowIndex].Value != "")
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
                amount = Val(qty) * price * (1 - Val(discountRatio) / 100);
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
                    dtgrdProductList[4, row].Value = LCurrency.displayValue(price.ToString);
                    dtgrdProductList[5, row].Value = LCurrency.displayValue(vat.ToString);
                    dtgrdProductList[6, row].Value = LCurrency.displayValue(discountRatio.ToString);
                    dtgrdProductList[7, row].Value = qty;
                    dtgrdProductList[8, row].Value = LCurrency.displayValue(amount.ToString);
                    dtgrdProductList[10, row].Value = description;
                    dtgrdProductList[0, row].ReadOnly = true;
                    dtgrdProductList[1, row].ReadOnly = true;
                    dtgrdProductList[2, row].ReadOnly = true;
                    seq = seq + 1;
                    AddToCart("", Till.TILLNO, dtgrdProductList.Item(0, row).Value, dtgrdProductList.Item(1, row).Value, dtgrdProductList.Item(2, row).Value, dtgrdProductList.Item(4, row).Value, dtgrdProductList.Item(5, row).Value, dtgrdProductList.Item(6, row).Value, dtgrdProductList.Item(7, row).Value, dtgrdProductList.Item(8, row).Value, dtgrdProductList.Item(10, row).Value);
                    if (dtgrdProductList.RowCount > 1)
                    {
                        if (dtgrdProductList.Item(7, row - 1).Value > 1)
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
                    if (dtgrdProductList[1, i].Value == "" | (double) dtgrdProductList[7, i].Value <= 0)
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
                    double price = (double) LCurrency.getValue(dtgrdProductList[4, i].Value.ToString());
                    double vat = (double) LCurrency.getValue(dtgrdProductList[5, i].Value.ToString());
                    double discountRatio = (double) LCurrency.getValue(dtgrdProductList[6, i].Value.ToString());
                    double qty = (double) dtgrdProductList[7, i].Value;
                    double amount = price * qty * (1d - discountRatio / 100d);
                    dtgrdProductList[8, i].Value = LCurrency.displayValue(amount.ToString());
                    if ((bool) dtgrdProductList[9, i].Value == false)
                    {
                        _total = _total + (double) LCurrency.getValue(dtgrdProductList[8, i].Value.ToString());
                        _vat = _vat + (double) (LCurrency.getValue(dtgrdProductList[5, i].Value.ToString())) * (double) LCurrency.getValue(dtgrdProductList[8, i].Value.ToString()) / (100 + (double) (LCurrency.getValue(dtgrdProductList[5, i].Value.ToString)));
                        double discountedPrice = (double) LCurrency.getValue(price.ToString()) * (1 - (double) discountRatio / 100d);
                        _discount = _discount + (price - discountedPrice) * qty;

                        // _discount = _discount + (Val(LCurrency.getValue(dtgrdProductList.Item(6, i).Value.ToString)) / 100) * Val(LCurrency.getValue(dtgrdProductList.Item(8, i).Value.ToString))
                    }
                }

                _grandTotal = _total;
                txtTotal.Text = LCurrency.displayValue(_total.ToString());
                txtDiscount.Text = LCurrency.displayValue(_discount.ToString());
                txtVAT.Text = LCurrency.displayValue(_vat.ToString());
                txtGrandTotal.Text = LCurrency.displayValue(_grandTotal.ToString());
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
                        if (frmAllow.allowed == true)
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
    }
}
