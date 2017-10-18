using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Assignment4
{
    public class DataService
    {
        public Category GetCategory(int i)
        {
            using (var db = new NorthwindContext())
            {
                var cat = db.Categories.FirstOrDefault(x => x.Id == i);
                return cat;
            }
        }

        public Product GetProduct(int i)
        {
            using (var db = new NorthwindContext())
            {
                var prod = db.Products.FirstOrDefault(x => x.Id == i);
                return prod;
            }
        }

        public Order GetOrder(int i)
        {
            using (var db = new NorthwindContext())
            {
                var ord = db.Orders.FirstOrDefault(x => x.Id == i);
                return ord;
            }
        }

        public List<Order> GetOrders()
        {
            using (var db = new NorthwindContext())
            {
                var ord = db.Orders.ToList();

                return ord;
            }
        }

        public List<OrderDetails> GetOrderDetailsByOrderId(int i)
        {
            throw new NotImplementedException();
        }

        public List<OrderDetails> GetOrderDetailsByProductId(int i)
        {
            throw new NotImplementedException();
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class OrderDetails : List<Order>
    {
        public int FKOrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Required { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public string ShipName { get; set; }
        public string ShipCity { get; set; }
        public Product Product { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public String QuantityPerUnit { get; set; }
        public int UnitsInStock { get; set; }
        public Category Category { get; set; }
    }
}
