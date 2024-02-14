using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Drawing;
using SimpleDbGui.Data;

namespace SimpleDbGui
{
    public partial class OrderView : Form
    {
        private int _customerId;

        public IDbConnection? Connection { get; set; }

        public bool IsConnected => Connection != null;

        public int CustomerId
        {
            get
            {
                return _customerId;
            }
            set
            {
                if (value != _customerId)
                {
                    _customerId = value;
                    RefreshData();
                }
            }
        }

        public OrderView()
        {
            InitializeComponent();
        }

        private void Orders_Shown(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            if (IsConnected)
            {
                gridOrders.DataSource = GetOrders(CustomerId);
            }
            else
            {
                gridOrders.DataSource = null;
            }
        }

        private IEnumerable<Order>? GetOrders(int customerId)
        {
            // TODO Query orders for the given customer from the database and fill a list of orders to return.
            //var cmd = (MySqlCommand)_conn.CreateCommand();
            //cmd.CommandText = $"SELECT FROM bestellungen WHERE kundennr = @kundennr;";
            //cmd.Parameters.AddWithValue("@kundennr", customerId);
            //Debug.WriteLine(cmd.CommandText);
            //var orders = new List<Order>();
            //using (var reader = cmd.ExecuteReader())
            //{
            //    while (reader.Read())
            //    {
            //        var order = new Order
            //        {
            //            OrderId = reader.GetInt32(0),
            //            CustomerId = reader.GetInt32(1),
            //            OrderDate = reader.GetDateTime(2),
            //            Total = reader.GetDecimal(3)
            //        };
            //        orders.Add(order);
            //    }
            //}
            return null;
        }
    }
}
