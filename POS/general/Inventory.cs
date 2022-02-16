using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.general
{
    class Inventory : Product
    {
        public double GL_MIN_INVENTORY = 0d;
        public double GL_MAX_INVENTORY = 0d;
        public double GL_REORDER_LEVEL = 0d;
        public double GL_DEFAULT_REORDER_QTY = 0d;

        public string getInventory(string itemCode)
        {
            string value = "";
            try
            {
                var conn = new MySqlConnection(Database.conString);
                var command = new MySqlCommand();
                // create bar code
                string codeQuery = "SELECT  `qty` FROM `inventorys` WHERE `item_code`='" + itemCode + "'";
                conn.Open();
                command.CommandText = codeQuery;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read)
                {
                    value = reader.GetString("qty");
                    break;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return value;
        }

        public bool adjustInventory(string itemCode, double value)
        {
            bool success = false;
            string actValue = value.ToString();
            try
            {
                var conn = new MySqlConnection(Database.conString);
                var command = new MySqlCommand();
                // create bar code
                string query = "UPDATE `inventorys` SET `qty`=`qty`+" + actValue + " WHERE `item_code`='" + itemCode + "'";
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                conn.Close();
                success = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return success;
        }

        public bool addInventory(string itemCode, double minInventory, double maxInventory, double reorderLevel, double defReorderQty)
        {
            bool added = false;
            return added;
        }

        public bool editInventory(string itemCode, double minInventory, double maxInventory, double reorderLevel, double defReorderQty)
        {
            bool edited = false;
            return edited;
        }
    }
}
