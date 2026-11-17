using SonarNet;

var customer = new Customer { Name = "Olena", IsVip = true };
var order = new Order { Id = 1, Country = "UA", PromoCode = "SALE10" };
order.Items.Add(new OrderItem { Name = "Book", Price = 250m, Quantity = 2 });
order.Items.Add(new OrderItem { Name = "Pen", Price = 20m, Quantity = 5 });

// Збираємо процесор із незалежних правил
var processor = new OrderProcessor(new OrderValidator(), new DiscountCalculator(), new ShippingCalculator());
var total = processor.ProcessOrder(order, customer);
Console.WriteLine(new ReportPrinter().Build(order, customer, total, processor.Log));
