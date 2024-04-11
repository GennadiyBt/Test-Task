using Test_Task.Models;


namespace Test_Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите номера заказов через запятую:");
            string orders = Console.ReadLine();
            Console.WriteLine();
            List<int> ordersIdList = orders.Split(',').Select(n => Convert.ToInt32(n)).ToList();
            List <Order> ordersList = new List<Order>();
            using (DataContext db = new DataContext())
            {               
                
                List<Product> products =  db.ProductsInOrders(ordersIdList);
                ordersList = db.Orders.Where(o => ordersIdList.Contains(o.Id)).ToList();
                List<int> productIds = products.Select(p => p.Id).ToList();
                var racks = db.Racks
                    .Where(r => r.RackProducts.Any(rp => productIds.Contains(rp.ProductId)))
                    .ToList();

                Dictionary<Product, Rack> mainRacks = new Dictionary<Product, Rack>();

                foreach (var product in products)
                {
                    var mainRack = db.RacksProduct
                        .Where(rp => rp.ProductId == product.Id && rp.Status == "Основной")
                        .Select(rp => rp.Rack)
                        .FirstOrDefault();

                    mainRacks.Add(product, mainRack);
                }

                var groupedProducts = mainRacks.GroupBy(kv => kv.Value, kv => kv.Key);
                var optimalRoute = groupedProducts.OrderByDescending(grp => grp.Count()).Select(grp => grp.Key);
                var orderedOprimalRoute = optimalRoute.OrderBy(r => r.Name).ToList();

                foreach (var item in orderedOprimalRoute) 
                {
                    PrintOrderDetails(item, ordersList, db);
                }
                
                void PrintOrderDetails(Rack rack, List<Order> orders, DataContext context)
                {
                    var mainRackId = rack.Id;
                    var productIds = context.RacksProduct
                        .Where(rp => rp.RackId == mainRackId && rp.Status == "Основной")
                        .Select(rp => rp.ProductId)
                        .ToList();
                    Console.WriteLine($"===Стеллаж {rack.Name}");
                    foreach (var order in orders)
                    {
                        foreach (var productId in productIds)
                        {
                            var orderProduct = context.OrderProducts
                                .Where(op => op.OrderId == order.Id && op.ProductId == productId)
                                .FirstOrDefault();

                        if (orderProduct != null)
                            {
                                var productName = context.Products.Where(p => p.Id == productId).Select(p => p.Name).FirstOrDefault();
                                Console.WriteLine($"{productName} (id={productId})\nзаказ: {order.Id}, {orderProduct.Quantity} шт");

                                var additionalRacks = context.RacksProduct
                                    .Where(rp => rp.ProductId == productId && rp.Status == "Дополнительный")
                                    .Select(rp => context.Racks.FirstOrDefault(r => r.Id == rp.RackId).Name)
                                    .ToList();

                                if (additionalRacks.Count > 0)
                                {
                                   Console.WriteLine("доп стеллаж: " + string.Join(", ", additionalRacks));
                                }

                                Console.WriteLine();
                            }
                        }
                    }                 
                }
            }
        }
    }
}
