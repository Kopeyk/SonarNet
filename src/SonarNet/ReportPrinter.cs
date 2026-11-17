using System.Text;

namespace SonarNet
{
    /// <summary>Формує текстовий звіт про замовлення. Отримує журнал як IEnumerable,
    /// тому більше не залежить від OrderProcessor.</summary>
    public class ReportPrinter
    {
        public string Build(Order order, Customer customer, decimal total, IEnumerable<string> log)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== ORDER REPORT ===");
            AppendHeader(sb, order, customer);
            AppendItems(sb, order);
            sb.AppendLine("Total: " + total);
            foreach (var line in log)
            {
                sb.AppendLine("log: " + line);
            }
            return sb.ToString();
        }

        private static void AppendHeader(StringBuilder sb, Order order, Customer customer)
        {
            sb.AppendLine("Order: " + order.Id);
            sb.AppendLine("Customer: " + customer.Name + (customer.IsVip ? " (VIP)" : ""));
            sb.AppendLine("Country: " + order.Country);
            sb.AppendLine("Promo: " + (order.PromoCode ?? "-"));
        }

        private static void AppendItems(StringBuilder sb, Order order)
        {
            foreach (var item in order.Items)
            {
                sb.AppendLine("  " + item.Name + " x" + item.Quantity + " = " + (item.Price * item.Quantity));
            }
        }
    }
}
