using Newtonsoft.Json.Linq;
using POS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace POS.general
{
    class LApplication
    {
        private static object getRemoteXMLFile(string path)
        {
            var document = new System.Xml.XmlDocument();
            // Create a WebRequest to the remote site
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest) System.Net.WebRequest.Create(path);

            // NB! Use the following line ONLY if the website is protected
            // request.Credentials = New System.Net.NetworkCredential("username", "password")

            // Call the remote site, and parse the data in a response object
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

            // Check if the response is OK (status code 200)
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                // Parse the contents from the response to a stream object
                Stream stream = response.GetResponseStream();
                // Create a reader for the stream object
                var reader = new StreamReader(stream);
                // Read from the stream object using the reader, put the contents in a string
                string contents = reader.ReadToEnd();
                // Create a new, empty XML document

                // Load the contents into the XML document
                document.LoadXml(contents);
            }

            // Now you have a XmlDocument object that contains the XML from the remote site, you can
            // use the objects and methods in the System.Xml namespace to read the document

            else
            {
                // If the call to the remote site fails, you'll have to handle this. There can be many reasons, ie. the 
                // remote site does not respond (code 404) or your username and password were incorrect (code 401)
                // 
                // See the codes in the System.Net.HttpStatusCode enumerator 

                throw new Exception("Could not retrieve document from the URL, response code: " + response.StatusCode);
            }

            return document;
        }
        public static string localAppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"QrbixPos";

        //public static string localAppDataDir = global::My.Computer.FileSystem.SpecialDirectories.MyDocuments + global::My.Application.Info.Title + "." + global::My.Application.Info.Version.Major.ToString + "." + global::My.Application.Info.Version.Minor.ToString;
        // Public Shared localAppDataDir As String = "C:\" + My.Application.Info.Title + "." + My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString
        private string databaseAddress = "";
        private string databasePassword = "";
        private string databaseUserID = "";
        private string databaseName = "";

        public object loadSettings()
        {
            string computerName = "";
            try
            {
                computerName = System.Environment.MachineName;
            }
            catch (Exception ex)
            {
            }

            bool loaded = false;
            string address = "";
            string path = localAppDataDir + @"\localSettings.xml";
            XmlReader document;
            try
            {
                if (File.Exists(path))
                {
                    document = new XmlTextReader(path);
                    while (document.Read())
                    {
                        var type = document.NodeType;
                        if (type == XmlNodeType.Element)
                        {
                            if (document.Name == "Address")
                            {
                                address = document.ReadInnerXml().ToString();
                            }
                        }
                    }

                    document.Dispose();
                }
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show("Could not load settings. Settings configuretions not found. Configure System?", "Settings not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    FormSetUp frmSetup = new FormSetUp();
                    frmSetup.ShowDialog();
                }
                MessageBox.Show("Could not load System. Application will close", "Error: Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                Environment.Exit(0);
            }

            var settings = new System.Xml.XmlDocument();
            try
            {
                settings = (System.Xml.XmlDocument)getRemoteXMLFile("http://" + address + "/rms/settings/setting_info.xml");
            }
            catch (System.Net.WebException ex)
            {
                DialogResult res = MessageBox.Show("Could not load settings. Settings configuretions not found. Configure System?", "Settings not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    FormSetUp frmSetup = new FormSetUp();
                    frmSetup.ShowDialog();
                }
                MessageBox.Show("Could not load System. Application will close", "Error: Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show("Could not load settings. Settings configuretions not found. Configure System?", "Settings not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    FormSetUp frmSetup = new FormSetUp();
                    frmSetup.ShowDialog();
                }
                MessageBox.Show("Could not load System. Application will close", "Error: Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                Environment.Exit(0);
            }

            // database infomation

            try
            {
                databaseName = settings.SelectSingleNode("Settings/Database/Name").InnerText.ToString();
                databaseAddress = settings.SelectSingleNode("Settings/Database/Address").InnerText.ToString();
                databaseUserID = settings.SelectSingleNode("Settings/Database/UserID").InnerText.ToString();
                databasePassword = settings.SelectSingleNode("Settings/Database/Password").InnerText.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            // load database

            string connectionString = "";
            connectionString = "server=" + databaseAddress + ";user id=" + databaseUserID + ";password=" + databasePassword + ";Database=" + databaseName + ";pooling=false";
            Database.conString = connectionString;
            var con = new MySqlConnection(Database.conString);
            bool isLoaded = false;
            try
            {
                isLoaded = true;
                con.Open();
                con.Close();
            }
            catch (Exception ex)
            {
                isLoaded = false;
                MessageBox.Show("Could not connect to database: " + ex.Message.ToString());
            }

            try
            {
                var response = new object();
                var json = new JObject();
                response = Web.get_("tills/get_by_computer_name?computer_name=" + System.Environment.MachineName);
                json = JObject.Parse(response.ToString());
                Till.TILLNO = json.SelectToken("no");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load System. Application will close", "Error: Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }







            // load till information
            try
            {
                string compName = System.Environment.MachineName;
                string query = "SELECT till.till_no,till.computer_name FROM `till` WHERE till.computer_name=@computerName";
                var command = new MySqlCommand();
                var conn = new MySqlConnection(Database.conString);
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@computerName", compName.ToString());
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read)
                    {
                        // Till.TILLNO = reader.GetString("till_no")
                        // Exit While
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message)
            }
            // load printer informations
            try
            {
                string compName = System.Environment.MachineName;
                string query = "SELECT till.till_no,till.computer_name,fiscal_printer.operator_code,fiscal_printer.operator_password,fiscal_printer.port,fiscal_printer.status FROM `till`,`fiscal_printer` WHERE till.till_no=fiscal_printer.till_no AND till.computer_name=@computerName";
                var command = new MySqlCommand();
                var conn = new MySqlConnection(Database.conString);
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@computerName", compName.ToString());
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read)
                    {
                        PointOfSale.operatorName = reader.GetString("operator_code");
                        PointOfSale.operatorPassword = reader.GetString("operator_password");
                        PointOfSale.port = reader.GetString("port");
                        PointOfSale.fiscalPrinterEnabled = reader.GetString("status");
                        break;
                    }
                }
                else
                {
                    Interaction.MsgBox("No fiscal printer settings", (MsgBoxStyle)((int)Constants.vbExclamation + (int)Constants.vbOKOnly), "Error: Fiscal Printer");
                }
            }
            catch (Exception ex)
            {
                // LError.databaseConnection()
            }

            try
            {
                string compName = System.Environment.MachineName;
                string query = "SELECT till.till_no,till.computer_name,pos_printer.logical_name,pos_printer.status FROM `till`,`pos_printer` WHERE till.till_no=pos_printer.till_no AND till.computer_name=@computerName";
                var command = new MySqlCommand();
                var conn = new MySqlConnection(Database.conString);
                conn.Open();
                command.CommandText = query;
                command.Connection = conn;
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@computerName", compName.ToString());
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read)
                    {
                        PointOfSale.posPrinterLogicName = reader.GetString("logical_name");
                        PointOfSale.posPrinterEnabled = reader.GetString("status");
                        break;
                    }
                }
                else
                {
                    Interaction.MsgBox("No POS printer settings", (MsgBoxStyle)((int)Constants.vbExclamation + (int)Constants.vbOKOnly), "Error: POS Printer");
                }
            }
            catch (Exception ex)
            {
                // LError.databaseConnection()
            }

            // load day information
            try
            {
                Day.systemDate = Day.getCurrentDay.ToString("yyyy-MM-dd"); // settings.SelectSingleNode("Settings/Day/Date").InnerText
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Could not load Day Information. Day not set.", (MsgBoxStyle)((int)Constants.vbExclamation + (int)Constants.vbOKOnly), "Error: Day error");
                Application.Exit();
                loaded = false;
            }

            loaded = true;
            if (Company.loadCompanyDetails == true)
            {
            }
            else
            {
                Interaction.MsgBox("Could not load company information", (MsgBoxStyle)((int)Constants.vbOKOnly + (int)Constants.vbCritical), "Error: Loading company information");
                loaded = false;
            }

            return loaded;
        }
    }
}
