namespace SonarNet
{
    /// <summary>Нова фіча: бонусні бали за замовлення – 1 бал за кожні повні 10 грн, VIP-клієнти отримують подвійні бали.</summary>
    public static class NewFeature
    {
        public static int LoyaltyPoints(decimal total, Customer customer)
        {
            if (total <= 0)
            {
                return 0;
            }
            int points = (int)(total / 10);
            return customer != null && customer.IsVip ? points * 2 : points;
        }
    }
}
