namespace SonarNet
{
    /// <summary>Розраховує суму замовлення: перевірка позицій, знижки, доставка.
    /// Залежить лише від інтерфейсів, тому окремі правила можна замінювати й тестувати незалежно.</summary>
    public class OrderProcessor
    {
        private readonly IOrderValidator _validator;
        private readonly IDiscountCalculator _discounts;
        private readonly IShippingCalculator _shipping;
        private readonly List<string> _log = new List<string>();

        public OrderProcessor(IOrderValidator validator, IDiscountCalculator discounts, IShippingCalculator shipping)
        {
            _validator = validator;
            _discounts = discounts;
            _shipping = shipping;
        }

        public IReadOnlyList<string> Log => _log;

        public decimal ProcessOrder(Order order, Customer customer)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            decimal total = SumValidItems(order);
            total = _discounts.Apply(total, customer);
            total += _shipping.Calculate(order.Country, total);
            total = _discounts.ApplyPromo(total, order.PromoCode);
            total = Math.Max(total, 0);

            _log.Add("order " + order.Id + " total " + total);
            return total;
        }

        // Сума лише коректних позицій; некоректні записуються в журнал
        private decimal SumValidItems(Order order)
        {
            decimal sum = 0;
            foreach (var item in order.Items)
            {
                if (_validator.IsValid(item, out var error))
                {
                    sum += item.Price * item.Quantity;
                }
                else
                {
                    _log.Add(error);
                }
            }
            return sum;
        }
    }
}
