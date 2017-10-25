using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DAL
{
    public class DataService : IDataService
    {   


        /*
        // Order
        /// <summary>
        /// Get a single order by ID
        /// </summary>
        /// <param name="id">The order Id</param>
        /// <returns>
        /// Return the complete order, i.e. all attributes of the order, the complete list of
        /// order details. Each order detail should include the product which must include
        /// the category
        /// </returns>
        public Order GetOrder(int id)
        {
            using (var db = new SovaContext())
            {
                var order = db.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Order)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ThenInclude(od => od.Category)
                    .FirstOrDefault(x => x.Id == id);
                return order;
            }
        }

        /// <summary>
        /// Get order by shipping name
        /// </summary>
        /// <param name="shipName">The shipping name</param>
        /// <returns>
        /// Return a list of orders with id, date, ship name and city.
        /// </returns>
        public List<Order> GetOrderByShipName(string shipName)
        {
            using (var db = new SovaContext())
            {
                var orders = db.Orders
                    .Where(x => x.ShipName.Equals(shipName))
                    .ToList();
                return orders;
            }
        }


        /// <summary>
        /// List all orders
        /// </summary>
        /// <returns>
        /// Return a list of orders with the same information as in 2.
        /// </returns>
        public List<Order> GetOrders()
        {

            using (var db = new SovaContext())
            {
                var orders = db.Orders.ToList();

                return orders;
            }
        }

        //Order Details
        /// <summary>
        /// Get the details for a specific order ID
        /// </summary>
        /// <param name="id">The order Id</param>
        /// <returns>
        /// Return the order details with product name, unit price, quantity.
        /// </returns>
        public List<OrderDetails> GetOrderDetailsByOrderId(int id)
        {
            using (var db = new SovaContext())
            {
                var details = db.OrderDetails
                    .Include(od => od.Product)
                    .Include(od => od.Order)
                    .Where(x => x.OrderId == id).ToList();
                
                return details;
            }
        }

        /// <summary>
        /// Get the details for a specific product ID
        /// </summary>
        /// <param name="id">The product Id</param>
        /// <returns>
        /// Return the complete list of details, with order date, unit price, quantity
        /// </returns>
        public List<OrderDetails> GetOrderDetailsByProductId(int id)
        {
            using (var db = new SovaContext())
            {

                
                var details = db.OrderDetails
                    .Include(od => od.Product)
                    .Include(od => od.Order)
                    .Where(x => x.ProductId == id).ToList();
                return details;
            }
        }

        // Product
        /// <summary>
        /// Get a single product by ID
        /// </summary>
        /// <param name="id">The product Id</param>
        /// <returns>The complete product with name, unit price and category name.</returns>
        public Product GetProduct(int id)
        {
            using (var db = new SovaContext())
            {
                var product = db.Products
                    .Include(p => p.Category)
                    .FirstOrDefault(x => x.Id == id);
                return product;
            }
        }

        public List<Product> GetProducts()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get products by category ID
        /// </summary>
        /// <param name="id">The category Id</param>
        /// <returns>
        /// Return the list of products with the given category. Return the same information as in 6
        /// </returns>
        public List<Product> GetProductByCategory(int id)
        {

            using (var db = new SovaContext())
            {
                var products = db.Products
                    .Include(p => p.Category)
                    .Where(x => x.CategoryId == id).ToList();
                return products;
            }
        }

        /// <summary>
        /// Get a list of products that contains a substring
        /// </summary>
        /// <param name="name">The string to search for</param>
        /// <returns>
        /// Search for products where the name matches the given substring. Return a list of product name and category name.
        /// </returns>
        public List<Product> GetProductByName(string name)
        {
            using (var db = new SovaContext())
            {
                var products = db.Products.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                return products;
            }
        }

        //Category
        /// <summary>
        /// Get Category by ID
        /// </summary>
        /// <param name="id">The category Id</param>
        /// <returns>Return the category if found otherwise return null.</returns>
        public Category GetCategory(int id)
        {
            using (var db = new SovaContext())
            {
                var cat = db.Categories.FirstOrDefault(x => x.Id == id);
                return cat;
            }
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>Return the list of categories with id, name and description.</returns>
        public List<Category> GetCategories()
        {
            using (var db = new SovaContext())
            {
                var cat = db.Categories.ToList();
                return cat;
            }
        }

        /// <summary>
        /// Add a new category to the system. The method takes name and description as
        /// arguments. The system must provide a new ID. 
        /// </summary>
        /// <param name="name">The name of the category to make</param>
        /// <param name="desc">The description of the category to make</param>
        /// <returns>
        /// return the newly created category.
        /// </returns>
        public Category CreateCategory(string name, string desc)
        {
            using (var db = new SovaContext())
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

        

        /// <summary>
        /// Take as arguments an id, name and description and update name and description. 
        /// </summary>
        /// <param name="id">The category Id</param>
        /// <param name="name">The categories new name</param>
        /// <param name="desc">The categoriers new description</param>
        /// <returns>
        /// If the category is found, update the category and return true, otherwise return false.
        /// </returns>
        public bool UpdateCategory(int id, string name, string desc)
        {
            using (var db = new SovaContext())
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

        /// <summary>
        /// Delete category. Take id as argument. 
        /// </summary>
        /// <param name="id">The category Id</param>
        /// <returns>
        /// Return true if the category is deleted, otherwise return false.
        /// </returns>
        public bool DeleteCategory(int id)
        {
            using (var db = new SovaContext())
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
        */
    }

    public class Post
    {
        public int post_id { get; set; }
        public int owner_id { get; set; }
        public int post_type_id { get; set; }
        public int parent_id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int score { get; set; }
        public DateTime closed_date { get; set; }
        public DateTime create_date { get; set; }
    }
    /*
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
    
        


    public static class Util
    {
        public static string ToJson(this object data)
        {
            return JsonConvert.SerializeObject(data,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
    */
}
