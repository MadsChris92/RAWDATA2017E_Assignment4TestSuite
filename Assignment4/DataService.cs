using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public List<Category> GetCategories()
        {
            using (var db = new NorthwindContext())
            {
                var cat = db.Categories.ToList();
                return cat;
            }
        }

        public Category AddCategory(string name, string desc)
        {
            using (var db = new NorthwindContext())
            {
                var cat = new Category
                {
                    Name = name,
                    Description = desc
                };

                db.Add(cat);
                db.SaveChanges();
                return cat;
            }
        }

        public bool RemoveCategory(int id)
        {
            using (var db = new NorthwindContext())
            {
                var category = GetCategory(id);
                if (category != null)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateCategory(int id, string name, string desc)
        {
            using (var db = new NorthwindContext())
            {

                var category = db.Categories.FirstOrDefault(x => x.Id == id);

                if (category != null)
                {
                    category.Name = name;
                    category.Description = desc;
                    db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }





        public Product GetProduct(int i)
        {
            using (var db = new NorthwindContext())
            {
            Product product;
                product = db.Products
                    .Include(p => p.Category)
                    .FirstOrDefault(x => x.Id == i);
            return product;
            }
        }

        public List<Product> GetProductByCatID(int i)
        {
            List<Product> products;
            using (var db = new NorthwindContext())
            {
                products = db.Products.Where(x => x.CategoryId == i).ToList();
            }
            return products;
        }

        public List<Product> GetProductBySubstring(string s)
        {
            using (var db = new NorthwindContext())
            {
                var products = db.Products.Where(x => x.Equals(s)).ToList();
                return products;
            }
        }

        public Order GetOrder(int i)
        {
            using (var db = new NorthwindContext())
            {
                var order = db.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Order)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ThenInclude(od => od.Category)
                    .FirstOrDefault(x => x.Id == i);
                return order;
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
            using(var db = new NorthwindContext())
            {
                var details = db.OrderDetails
                    .Include(od => od.Product)
                    .Include(od => od.Order)
                    .Where(x => x.OrderId == i).ToList();
                return details;
            }
        }

        public List<OrderDetails> GetOrderDetailsByProductId(int i)
        {
            using (var db = new NorthwindContext())
            {
            List<OrderDetails> details;
                details = db.OrderDetails
                    .Include(od => od.Product)
                    .Include(od => od.Order)
                    .Where(x => x.ProductId == i).ToList();
            return details;
            }
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class OrderDetails
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }

    public class Order
    {
        [Column("OrderId")]public int Id { get; set; }
        public string CustomerId { get; set; }
        public int EmployeeId { get; set; }
        [Column("OrderDate")]public DateTime Date { get; set; }
        [Column("RequiredDate")]public DateTime Required { get; set; }
        [Column("ShippedDate")]public DateTime? Shipped { get; set; }
        public double Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        
        public virtual List<OrderDetails> OrderDetails { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public string QuantityPerUnit { get; set; }
        public int UnitsInStock { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
