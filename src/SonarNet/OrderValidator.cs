namespace SonarNet
{
    /// <summary>Відкидає позиції з некоректною кількістю або ціною.</summary>
    public class OrderValidator : IOrderValidator
    {
        public bool IsValid(OrderItem item, out string error)
        {
            if (item.Quantity <= 0)
            {
                error = "bad qty " + item.Name;
                return false;
            }
            if (item.Price < 0)
            {
                error = "bad price " + item.Name;
                return false;
            }
            error = null;
            return true;
        }
    }
}
