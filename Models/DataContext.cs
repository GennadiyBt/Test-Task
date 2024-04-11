using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Test_Task.Models
{
    internal class DataContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Rack> Racks => Set<Rack>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<RackProduct> RacksProduct => Set<RackProduct>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();

        public DataContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=test_data_base.db"); 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });
            modelBuilder.Entity<RackProduct>()
                .HasKey(op => new { op.RackId, op.ProductId });
        }
        
        internal List<Product> ProductsInOrders(List<int> ordersList)
        {
            var products = Products
                    .Join(OrderProducts, product => product.Id, orderProduct => orderProduct.ProductId,
                          (product, orderProduct) => new { Product = product, OrderProduct = orderProduct })
                    .Where(p => ordersList.Contains(p.OrderProduct.OrderId))
                    .Select(p => p.Product)
                    .Distinct()
                    .ToList();
            return products;
        }
        
    }
}
