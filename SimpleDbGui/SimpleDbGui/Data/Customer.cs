namespace SimpleDbGui.Data
{
    public class Customer
    {
        public static readonly Customer NONE = new Customer()
        {
            Id = -1
        };

        private static int _id = 0;

        public int Id { get; set; } = ++_id;
        public string Name { get; set; } = "?";
        public string Firstname { get; set; } = "?";
        public string Street { get; set; } = "?";
        public string ZipCode { get; set; } = "?";
        public string City { get; set; } = "?";
        public List<Order> Orders { get; } = new List<Order>();

        public int OrderCount => Orders.Count;
    }
}