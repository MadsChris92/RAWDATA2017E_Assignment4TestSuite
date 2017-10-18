using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

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
                var prod = db.Products.FirstOrDefault(x => x.Id == i);
                return prod;
            }
        }

        public List<Product> GetProductByCatID(int i)
        {
            using (var db = new NorthwindContext())
            {
                var products = db.Products.Where(x => x.CategoryId == i).ToList();
                return products;
            }
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
            /*
            using (var db = new NorthwindContext())
            {
                
                var ord = db.Orders.FirstOrDefault(x => x.Id == i);
                return ord;
            }
            */
            return new Order();
        }

        public List<Order> GetOrders()
        {
            /*
            using (var db = new NorthwindContext())
            {
                var ord = db.Orders.ToList();

                return ord;
            }
            */
            return new List<Order>();
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
        private int _categoryId;
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public String QuantityPerUnit { get; set; }
        public int UnitsInStock { get; set; }
        public Category Category
        {
            get
            {
                DataService d = new DataService();
                return d.GetCategory(_categoryId);
            }
            set { throw new NotImplementedException(); }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set
            {
                DataService d = new DataService();
                Category = d.GetCategory(value);
                
                _categoryId = value;
            }
        }
    }
}
