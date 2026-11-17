namespace SonarNet
{
    public class Customer
    {
        public string Name { get; set; }
        public bool IsVip { get; set; }
    }

    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string PromoCode { get; set; }
        public List<OrderItem> Items { get; } = new List<OrderItem>();
    }
}
