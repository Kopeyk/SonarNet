namespace SonarNet
{
    /// <summary>Знижки: VIP 15 % (понад 1000) або 10 %, звичайний клієнт 5 % понад 2000,
    /// промокод SALE* знімає 100 грн із суми (разом із доставкою) понад 300.</summary>
    public class DiscountCalculator : IDiscountCalculator
    {
        public decimal Apply(decimal total, Customer customer) => total * Rate(total, customer);

        public decimal ApplyPromo(decimal total, string promoCode) =>
            IsSalePromo(promoCode) && total > 300 ? total - 100 : total;

        // Коефіцієнт знижки залежно від статусу клієнта і суми
        private static decimal Rate(decimal total, Customer customer)
        {
            if (customer.IsVip)
            {
                return total > 1000 ? 0.85m : 0.9m;
            }
            return total > 2000 ? 0.95m : 1m;
        }

        private static bool IsSalePromo(string promoCode) =>
            promoCode != null && promoCode.StartsWith("SALE");
    }
}
