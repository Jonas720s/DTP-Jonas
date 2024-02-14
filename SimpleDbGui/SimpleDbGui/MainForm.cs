using MySql.Data.MySqlClient;
using SimpleDbGui.Data;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace SimpleDbGui
{
    public partial class MainForm : Form
    {
        private object _lock = new object();
        private IDbConnection? _conn;

        private OrderView _orderDialog = new OrderView();

        public bool IsConnected => _conn != null;

        protected Customer? CurrentCustomer => (gridCustomers.CurrentRow != null)  && IsConnected
                                               ? gridCustomers.CurrentRow.DataBoundItem as Customer 
                                               : null;

        #region Prepared framework stuff
        public MainForm()
        {
            InitializeComponent();

            string password = File.ReadAllText("C:\\git\\SimpleDbGui\\password.txt");

            txtHostname.Text = LoadFromSettingsOrDefault("Host", "localhost");
            txtUsername.Text = LoadFromSettingsOrDefault("User", "root");
            txtPassword.Text = LoadFromSettingsOrDefault("Pass", password);

            UpdateConnectionStatus();
        }

        private string LoadFromSettingsOrDefault(string key, string defaultValue)
        {
            var result = defaultValue;

            try
            {
                var valueRead = ConfigurationManager.AppSettings[key];
                result = string.IsNullOrWhiteSpace(valueRead) ? defaultValue : valueRead;
            }
            catch
            {
            }

            return result;
        }

        private string BuildConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder()
            {
                Server = txtHostname.Text,
                UserID = txtUsername.Text,
                Password = txtPassword.Text,
            };

            var additions = LoadFromSettingsOrDefault("AdditionalConnectionString", "");
            if (!string.IsNullOrEmpty(additions))
            {
                return builder.ConnectionString + $";{additions}";
            }

            return builder.ConnectionString;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _conn = new MySqlConnection(BuildConnectionString());
                _orderDialog.Connection = _conn;
                _conn.Open();
                _conn.ChangeDatabase("sqlteacherdb");
                DisplayCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Couldn't connect to database.\n\n{0}", ex.Message));
            }

            UpdateConnectionStatus();
        }

        private void DisplayCustomers()
        {
            if (!IsConnected)
            {
                gridCustomers.DataSource = null;
                return;
            }

            gridCustomers.DataSource = GetCustomers();
            gridCustomers.Columns.Clear();
            gridCustomers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
            });
            gridCustomers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Firstname",
                HeaderText = "Firstname",
            });
            gridCustomers.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "OrderCount",
                HeaderText = "Order Count",
            });
            FillUiFromCustomer(CurrentCustomer);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    _orderDialog.Connection = null;
                    _conn?.Close();
                    _conn?.Dispose();
                    _conn = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Couldn't disconnect from database.\n\n{0}", ex.Message));
                }

                UpdateConnectionStatus();
                DisplayCustomers();
            }
        }

        private void UpdateConnectionStatus()
        {
            btnConnect.Enabled = !IsConnected;
            btnDisconnect.Enabled = IsConnected;
            grpCustomer.Enabled = IsConnected;

            if (!IsConnected)
            {
                EmptyTextboxes(grpCustomer.Controls);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddOrUpdateCustomer();
            DisplayCustomers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CurrentCustomer != null && IsConnected)
            {
                var customerToDelete = CurrentCustomer;
                // Avoid updating issues with the selection
                SelectPreviousRecordIfLastSelected();
                RemoveCustomer(customerToDelete);
                DisplayCustomers();
            }
        }

        private void SelectPreviousRecordIfLastSelected()
        {
            var rowIndex = gridCustomers.CurrentRow.Index;
            if (rowIndex == gridCustomers.Rows.Count - 1)
            {
                gridCustomers.ClearSelection();
                if (rowIndex > 0)
                {
                    gridCustomers.CurrentCell = gridCustomers.Rows[rowIndex - 1].Cells[0];
                }
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            FillUiFromCustomer(CurrentCustomer);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                EmptyTextboxes(grpCustomer.Controls);
                gridCustomers.ClearSelection();
            }
        }

        private void btnShowOrders_Click(object sender, EventArgs e)
        {
            if (CurrentCustomer != null)
            {
                _orderDialog.CustomerId = CurrentCustomer.Id;
                _orderDialog.ShowDialog();
            }
        }

        private void gridCustomers_SelectionChanged(object sender, EventArgs e)
        {
            FillUiFromCustomer(CurrentCustomer);
        }
        #endregion

        private void FillCustomerFromUi(Customer customer)
        {
            customer.Name = txtName.Text;
            customer.Firstname = txtFirstname.Text;
            customer.Street = txtStreet.Text;
            customer.ZipCode = txtZip.Text;
            customer.City = txtCity.Text;
        }

        private void FillUiFromCustomer(Customer? customer)
        {
            if (customer != null)
            {
                txtID.Text = customer.Id.ToString();
                txtName.Text = customer.Name;
                txtFirstname.Text = customer.Firstname;
                txtStreet.Text = customer.Street;
                txtZip.Text = customer.ZipCode;
                txtCity.Text = customer.City;
            }
            else
            {
                EmptyTextboxes(grpCustomer.Controls);
            }
        }

        private void EmptyTextboxes(Control.ControlCollection controls)
        {
            foreach (var control in controls)
            {
                var box = control as TextBox;
                if (box != null)
                {
                    box.Text = "";
                }
            }
        }

        private IEnumerable<Customer>? GetCustomers()
        {
            //TODO Query customers from the database and fill a list of customers to return.
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM kunde";
            
            var customers = new List<Customer>();

            using (var reader = cmd.ExecuteReader()) {                 
                while (reader.Read())
                {
                    var customer = new Customer();
                    customer.Id = reader.GetInt32(0);
                    customer.Name = reader.GetString(1);
                    customer.Firstname = reader.GetString(2);
                    customer.Street = reader.GetString(3);
                    customer.ZipCode = reader.GetString(4);
                    customer.City = reader.GetString(5);
                    customers.Add(customer);
                }
            }

            return customers;
        }

        private void AddOrUpdateCustomer()
        {
            // TODO Add code to add the given customer to the database if not already there or
            //      update it if the customer is already existing. Display an error if the customer
            //      is not existing anymore or if you have a collission in changes
            if (!string.IsNullOrEmpty(txtID.Text))
            {
                var updateCmd = (MySqlCommand)_conn.CreateCommand();
                updateCmd.CommandText = $"UPDATE kunde SET name = @name, vorname = @vorname, " +
                    $"strasse = @strasse, plz = @plz, ort = @ort WHERE kundennr = @kundennr;";
                updateCmd.Parameters.AddWithValue("@name", txtName.Text);
                updateCmd.Parameters.AddWithValue("@vorname", txtFirstname.Text);
                updateCmd.Parameters.AddWithValue("@strasse", txtStreet.Text);
                updateCmd.Parameters.AddWithValue("@plz", txtZip.Text);
                updateCmd.Parameters.AddWithValue("@ort", txtCity.Text);
                updateCmd.Parameters.AddWithValue("@kundennr", txtID.Text);
                Debug.WriteLine(updateCmd.CommandText);
                updateCmd.ExecuteNonQuery();
            }
            else
            {
                int newId;
                var cmd = _conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(kundennr) FROM kunde";
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    newId = reader.GetInt32(0) + 1;
                }
                var insertCmd = (MySqlCommand)_conn.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO kunde" +
                $"(kundennr, name, vorname, strasse, plz, ort) " +
                $"VALUES ({newId},@name,@vorname,@strasse,@plz,@ort);";
                insertCmd.Parameters.AddWithValue("@name", txtName.Text);
                insertCmd.Parameters.AddWithValue("@vorname", txtFirstname.Text);
                insertCmd.Parameters.AddWithValue("@strasse", txtStreet.Text);
                insertCmd.Parameters.AddWithValue("@plz", txtZip.Text);
                insertCmd.Parameters.AddWithValue("@ort", txtCity.Text);
                Debug.WriteLine(insertCmd.CommandText);
                insertCmd.ExecuteNonQuery();
            }
        }

        private void RemoveCustomer(Customer customer)
        {
            // TODO Add code to remove the selected customer from the database
            var cmd = (MySqlCommand)_conn.CreateCommand();
            cmd.CommandText = $"DELETE FROM kunde WHERE kundennr = @kundennr;";
            cmd.Parameters.AddWithValue("@kundennr", customer.Id);
            Debug.WriteLine(cmd.CommandText);
            cmd.ExecuteNonQuery();
        }
    }
}