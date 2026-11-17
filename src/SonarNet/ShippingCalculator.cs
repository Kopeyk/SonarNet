namespace SonarNet
{
    /// <summary>Доставка: Україна безкоштовно від 500 грн, ЄС 250/100 грн, інші країни 400 грн.</summary>
    public class ShippingCalculator : IShippingCalculator
    {
        private static readonly HashSet<string> EuCountries = new HashSet<string> { "PL", "DE" };

        public decimal Calculate(string country, decimal total)
        {
            if (country == "UA")
            {
                return total < 500 ? 70 : 0;
            }
            if (EuCountries.Contains(country))
            {
                return total < 1000 ? 250 : 100;
            }
            return 400;
        }
    }
}
