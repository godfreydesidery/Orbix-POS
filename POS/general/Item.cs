using POS.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.general
{
    class Item
    {
        // Install-Package Microsoft.VisualBasic

        
        public string getShortDescription(string itemCode)
        {
            string descr = "";
            string query = "SELECT`item_description` FROM `items` WHERE `item_code`='" + itemCode + "'";
            var command = new MySqlCommand();
            var conn = new MySqlConnection(Database.conString);
            try
            {
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                MySqlDataReader reader = command.ExecuteReader;
                while (reader.Read)
                {
                    descr = reader.GetString("item_description");
                    break;
                }
            }
            catch (Exception ex)
            {
                LError.databaseConnection();
            }

            return descr;
        }

        public static string getCostPrice(string itemCode)
        {
            double price = 0d;
            string query = "SELECT`unit_cost_price` FROM `items` WHERE `item_code`='" + itemCode + "'";
            var command = new MySqlCommand();
            var conn = new MySqlConnection(Database.conString);
            try
            {
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                MySqlDataReader reader = command.ExecuteReader;
                while (reader.Read)
                {
                    price = reader.GetString("unit_cost_price");
                    break;
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
                Error.databaseConnection();
            }

            return price.ToString();
        }

        public List<string> getItems(string descr)
        {
            var list = new List<string>();
            try
            {
                // Dim query As String = "SELECT `items`.`item_code`,`items`.`item_long_description`, `inventorys`.`item_code`FROM `items`,`inventorys` WHERE  `items`.`item_long_description` LIKE CONCAT('%','" + descr + "','%') LIMIT 1,500"
                string query = "SELECT `item_long_description` FROM `items`";
                var command = new MySqlCommand();
                var conn = new MySqlConnection(Database.conString);
                try
                {
                    list.Clear();
                    conn.Open();
                    command.CommandText = query;
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;
                    MySqlDataReader itemreader = command.ExecuteReader();
                    if (itemreader.HasRows == true)
                    {
                        while (itemreader.Read)
                            list.Add(itemreader("item_long_description").ToString);
                    }
                    else
                    {
                        return list;
                    }
                }
                catch (Devart.Data.MySql.MySqlException ex)
                {
                    Interaction.MsgBox(ex.Message);
                    return list;
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.StackTrace.ToString());
            }

            return list;
        }

        public List<string> getItemDescriptions()
        {
            var list = new List<string>();
            try
            {
                // Dim query As String = "SELECT `items`.`item_code`,`items`.`item_long_description`, `inventorys`.`item_code`FROM `items`,`inventorys` WHERE  `items`.`item_long_description` LIKE CONCAT('%','" + descr + "','%') LIMIT 1,500"
                string query = "SELECT `item_long_description` FROM `items`";
                var command = new MySqlCommand();
                var conn = new MySqlConnection(Database.conString);
                try
                {
                    list.Clear();
                    conn.Open();
                    command.CommandText = query;
                    command.Connection = conn;
                    command.CommandType = CommandType.Text;
                    MySqlDataReader itemreader = command.ExecuteReader();
                    if (itemreader.HasRows == true)
                    {
                        while (itemreader.Read)
                            list.Add(itemreader("item_long_description").ToString);
                    }
                    else
                    {
                        return list;
                        return default;
                    }
                }
                catch (Devart.Data.MySql.MySqlException ex)
                {
                    Interaction.MsgBox(ex.Message);
                    return list;
                    return default;
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.StackTrace.ToString());
            }

            return list;
        }
        
    }
}
