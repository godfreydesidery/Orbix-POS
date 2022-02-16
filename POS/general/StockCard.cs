using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class StockCard
    {
        public bool qtyIn(string date_, string itemCode, double qty, double balance, string reference)
        {
            bool success = false;
            try
            {
                var conn = new MySqlConnection(Database.conString);
                var command = new MySqlCommand();
                string query = "";
                query = "INSERT INTO `stock_cards`(`date`,`item_code`,`qty_in`,`balance`,`reference`) VALUES (@date,@item_code,@qty,@balance,@reference)";
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@date", date_);
                command.Parameters.AddWithValue("@item_code", itemCode);
                command.Parameters.AddWithValue("@qty", qty);
                command.Parameters.AddWithValue("@balance", balance);
                command.Parameters.AddWithValue("@reference", reference);
                command.ExecuteNonQuery();
                conn.Close();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Interaction.MsgBox(ex.Message);
            }

            return success;
        }

        public bool qtyOut(string date_, string itemCode, double qty, double balance, string reference)
        {
            bool success = false;
            try
            {
                var conn = new MySqlConnection(Database.conString);
                var command = new MySqlCommand();
                string query = "";
                query = "INSERT INTO `stock_cards`(`date`,`item_code`,`qty_out`,`balance`,`reference`) VALUES (@date,@item_code,@qty,@balance,@reference)";
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@date", date_);
                command.Parameters.AddWithValue("@item_code", itemCode);
                command.Parameters.AddWithValue("@qty", qty);
                command.Parameters.AddWithValue("@balance", balance);
                command.Parameters.AddWithValue("@reference", reference);
                command.ExecuteNonQuery();
                conn.Close();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Interaction.MsgBox(ex.Message);
            }

            return success;
        }
    }
}
