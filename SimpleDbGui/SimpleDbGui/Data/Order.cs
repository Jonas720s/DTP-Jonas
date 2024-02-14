using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDbGui.Data
{
    public class Order
    {
        public static readonly Order NONE = new Order()
        {
            Id = -1
        };

        private static int _id = 0;
        public int Id { get; set; } = ++_id;
        public Customer Customer { get; init; } = Customer.NONE;
        public DateTime OrderDate { get; set; } = DateTime.MinValue;
        public DateTime? DeliveryDate { get; set; }
        public double Balance { get; set; } = 0;

    }
}
