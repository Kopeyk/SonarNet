namespace SonarNet
{
    /// <summary>Перевірка позиції замовлення.</summary>
    public interface IOrderValidator
    {
        bool IsValid(OrderItem item, out string error);
    }

    /// <summary>Розрахунок знижок для клієнта та промокоду.</summary>
    public interface IDiscountCalculator
    {
        decimal Apply(decimal total, Customer customer);
        decimal ApplyPromo(decimal total, string promoCode);
    }

    /// <summary>Розрахунок вартості доставки за країною.</summary>
    public interface IShippingCalculator
    {
        decimal Calculate(string country, decimal total);
    }
}
