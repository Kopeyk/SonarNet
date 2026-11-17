using SonarNet;

namespace SonarNet.Tests
{
    public class DiscountCalculatorTests
    {
        private readonly DiscountCalculator _calc = new DiscountCalculator();

        [Theory]
        [InlineData(1200, true, 1020)]   // VIP понад 1000 грн: -15 %
        [InlineData(1000, true, 900)]    // VIP до 1000 грн включно: -10 %
        [InlineData(2500, false, 2375)]  // звичайний клієнт понад 2000 грн: -5 %
        [InlineData(500, false, 500)]    // звичайний клієнт: без знижки
        public void Apply_ReturnsDiscountedTotal(decimal total, bool vip, decimal expected)
        {
            Assert.Equal(expected, _calc.Apply(total, new Customer { IsVip = vip }));
        }

        [Theory]
        [InlineData(540, "SALE10", 440)]
        [InlineData(250, "SALE10", 250)] // сума не більша за 300 – промокод не діє
        [InlineData(540, "PROMO", 540)]
        [InlineData(540, null, 540)]
        public void ApplyPromo_SubtractsOnlyForSalePromo(decimal total, string promo, decimal expected)
        {
            Assert.Equal(expected, _calc.ApplyPromo(total, promo));
        }
    }

    public class ShippingCalculatorTests
    {
        [Theory]
        [InlineData("UA", 400, 70)]
        [InlineData("UA", 500, 0)]
        [InlineData("PL", 900, 250)]
        [InlineData("DE", 1500, 100)]
        [InlineData("US", 100, 400)]
        public void Calculate_DependsOnCountryAndTotal(string country, decimal total, decimal expected)
        {
            Assert.Equal(expected, new ShippingCalculator().Calculate(country, total));
        }
    }

    public class OrderValidatorTests
    {
        [Fact]
        public void IsValid_RejectsZeroQuantity()
        {
            var ok = new OrderValidator().IsValid(new OrderItem { Name = "Pen", Price = 10, Quantity = 0 }, out var error);
            Assert.False(ok);
            Assert.Equal("bad qty Pen", error);
        }

        [Fact]
        public void IsValid_AcceptsCorrectItem()
        {
            Assert.True(new OrderValidator().IsValid(new OrderItem { Name = "Book", Price = 250, Quantity = 1 }, out var error));
            Assert.Null(error);
        }
    }

    public class OrderProcessorTests
    {
        private static OrderProcessor Create() =>
            new OrderProcessor(new OrderValidator(), new DiscountCalculator(), new ShippingCalculator());

        [Fact]
        public void ProcessOrder_CalculatesSampleOrder()
        {
            var order = new Order { Id = 1, Country = "UA", PromoCode = "SALE10" };
            order.Items.Add(new OrderItem { Name = "Book", Price = 250m, Quantity = 2 });
            order.Items.Add(new OrderItem { Name = "Pen", Price = 20m, Quantity = 5 });
            order.Items.Add(new OrderItem { Name = "Broken", Price = 5m, Quantity = 0 });
            var processor = Create();

            var total = processor.ProcessOrder(order, new Customer { Name = "Olena", IsVip = true });

            Assert.Equal(440m, total);
            Assert.Contains("bad qty Broken", processor.Log);
        }

        [Fact]
        public void ProcessOrder_ThrowsForNullOrder()
        {
            Assert.Throws<ArgumentNullException>(() => Create().ProcessOrder(null, new Customer()));
        }
    }
}
