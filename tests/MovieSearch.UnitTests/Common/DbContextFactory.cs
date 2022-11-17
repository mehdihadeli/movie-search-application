// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Orders.Core.Aggregates.Orders;
// using Orders.Infrastructure;
// using Orders.UnitTests.TestSeeds;
//
// namespace Orders.UnitTests.Common
// {
//     public static class DbContextFactory<T> where T: DbContext
//     {
//         public static async Task<T> Create()
//         {
//             var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
//
//             var context = new ApplicationDbContext(options);
//
//             // Seed our data
//             await SeedProducts(context);
//
//             await SeedOrders(context);
//
//             return context;
//         }
//
//         public static async Task Destroy(ApplicationDbContext context)
//         {
//             await context.Database.EnsureDeletedAsync();
//             await context.DisposeAsync();
//         }
//
//         private static async Task SeedProducts(ApplicationDbContext context)
//         {
//             foreach (var product in ProductsTestSeed.ProductsSeedData)
//             {
//                 var exist = await context.Products.AsQueryable().AnyAsync(x => x.Id == product.Id);
//                 if (exist == false)
//                     await context.Products.AddAsync(product);
//             }
//
//             await context.SaveChangesAsync();
//         }
//
//
//         private static async Task SeedOrders(ApplicationDbContext context)
//         {
//             var products = context.Products.ToList();
//
//             var order1 = new Order(Guid.NewGuid());
//             order1.ChangeOrderItems(new List<OrderItem>()
//             {
//                 new(products[0], 3)
//             });
//             order1.CalculateRequiredBinWidth();
//
//             var order2 = new Order(Guid.NewGuid());
//             order2.ChangeOrderItems(new List<OrderItem>()
//             {
//                 new(products[1], 5)
//             });
//             order2.CalculateRequiredBinWidth();
//
//             await context.AddRangeAsync(order1, order2);
//
//             await context.SaveChangesAsync();
//         }
//     }
// }

