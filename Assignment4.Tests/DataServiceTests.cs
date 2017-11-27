using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Xunit;

namespace Assignment4.Tests
{
    public class DataServiceTests
    {
        private const int ValidPostId = 841339;

        /* Search */

        [Fact]
        public void GetQuestionByTitle_ArgumentWithResults_ReturnsListOfSearchQuestion()
        {
            var service = new DataService();
            var results = service.GetQuestionByTitle("sql", 0, 10, out var totalResults);
            Assert.Equal(84, totalResults);
            Assert.Equal(ValidPostId, results.First().Id);
        }

        [Fact]
        public void GetQuestionByTag_ArgumentWithResults_ReturnsListOfSearchQuestion()
        {
            var service = new DataService();
            var results = service.GetQuestionByTag("sql", 0, 10, out var totalResults);
            Assert.Equal(52, totalResults);
            Assert.Equal(60174, results.First().Id);
        }

        /* Marking */

        [Fact]
        public void MarkPost_ValidId_ReturnsTrue()
        {
            var service = new DataService();
            var result = service.MarkPost(ValidPostId);
            Assert.True(result);

            //Cleanup
            service.UnmarkPost(ValidPostId);
        }

        [Fact]
        public void MarkPost_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.MarkPost(-1);
            Assert.False(result);
        }

        [Fact]
        public void UnmarkPost_ValidId_ReturnsTrue()
        {
            var service = new DataService();
            service.MarkPost(ValidPostId);

            var result = service.UnmarkPost(ValidPostId);
            Assert.True(result);
        }

        [Fact]
        public void UnmarkPost_InvalidId_ReturnsFalse()
        {
            var service = new DataService();

            var result = service.UnmarkPost(-1);
            Assert.False(result);
        }

        /* History */

        [Fact]
        public void AddHistory_ValidString_ReturnsTrue()
        {
            var service = new DataService();
            var result = service.AddHistory("test");
            Assert.True(result);
        }

        [Fact]
        public void AddHistory_Null_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.AddHistory(null);
            Assert.False(result);
        }

        [Fact]
        public void GetHistory__ReturnsListOfHistory()
        {
            var service = new DataService();
            service.ClearHistory();
            service.AddHistory("item1");
            service.AddHistory("item2");

            var result = service.GetHistory(0, 10, out var totalResults);
            Assert.NotNull(result);
            Assert.Equal(2, totalResults);
            Assert.Contains(result, history => history.Text == "item1");

            //Clean
            service.ClearHistory();
        }

        /*
        [Fact]
        public void GetHistory_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.GetHistory(-1);
            Assert.False(result);
        }

        [Fact]
        public void GetHistory_ValidId_ReturnsTrue()
        {
            var service = new DataService();
            service.ClearHistory();
            service.AddHistory("item1");
            var id = service.GetHistory().First().Id;
            var result = service.GetHistory(id);
            Assert.True(result);
        }

        [Fact]
        public void DeleteHistory_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.DeleteHistory(-1);
            Assert.False(result);
        }

        [Fact]
        public void DeleteHistory_ValidId_ReturnsTrue()
        {
            var service = new DataService();
            service.ClearHistory();
            service.AddHistory("item1");
            var id = service.GetHistory().First().Id;
            var result = service.DeleteHistory(id);
            Assert.True(result);
        }
        */

        /* Notes */

        //Create
        [Fact]
        public void CreateNote_ValidPostId_Note()
        {
            var service = new DataService();
            var result = service.CreateNote(ValidPostId, "Make note of this");
            
            Assert.NotNull(result);

            var notes = service.GetNotes(ValidPostId, 0, 100, out var _);

            Assert.Contains(notes, note => note.Text == "Make note of this");

            //Clean
            notes.ForEach(note => service.DeleteNote(note.Id));

        }

        [Fact]
        public void CreateNote_InvalidPostId_Null()
        {
            var service = new DataService();
            var result = service.CreateNote(-1, "Make null of this");

            Assert.Null(result);
        }
        //Read
        [Fact]
        public void ReadNote_ValidPostId_Note()
        {
            var service = new DataService();
            service.CreateNote(ValidPostId, "Make note of this");

            var notes = service.GetNotes(ValidPostId, 0, 100, out var _);

            Assert.Contains(notes, note => note.Text == "Make note of this");

            //Clean
            notes.ForEach(note => service.DeleteNote(note.Id));

        }
        //Update
        //Delete

        /*
        [Fact]
        public void DeleteCategory_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.DeleteCategory(-1);
            Assert.False(result);
        }

        [Fact]
        public void UpdateCategory_NewNameAndDescription_UpdateWithNewValues()
        {
            var service = new DataService();
            var category = service.CreateCategory("TestingUpdate", "UpdateCategory_NewNameAndDescription_UpdateWithNewValues");

            var result = service.UpdateCategory(category.Id, "UpdatedName", "UpdatedDescription");
            Assert.True(result);

            category = service.GetCategory(category.Id);

            Assert.Equal("UpdatedName", category.Name);
            Assert.Equal("UpdatedDescription", category.Description);

            // cleanup
            service.DeleteCategory(category.Id);
        }

        [Fact]
        public void UpdateCategory_InvalidID_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.UpdateCategory(-1, "UpdatedName", "UpdatedDescription");
            Assert.False(result);
        }
        */
        /* products */
        /*
        [Fact]
        public void Product_Object_HasIdNameUnitPriceQuantityPerUnitAndUnitsInStock()
        {
            var product = new Product();
            Assert.Equal(0, product.Id);
            Assert.Null(product.Name);
            Assert.Equal(0.0, product.UnitPrice);
            Assert.Null(product.QuantityPerUnit);
            Assert.Equal(0, product.UnitsInStock);
        }

        [Fact]
        public void GetProduct_ValidId_ReturnsProductWithCategory()
        {
            var service = new DataService();
            var product = service.GetProduct(1);
            Assert.Equal("Chai", product.Name);
            Assert.Equal("Beverages", product.Category.Name);
        }

        [Fact]
        public void GetProduct_NameSubString_ReturnsProductsThatMachesTheSubString()
        {
            var service = new DataService();
            var products = service.GetProductByName("ant");
            Assert.Equal(3, products.Count);
            Assert.Equal("Chef Anton's Cajun Seasoning", products.First().Name);
            Assert.Equal("Guaraná Fantástica", products.Last().Name);
        }

        [Fact]
        public void GetProductsByCategory_ValidId_ReturnsProductWithCategory()
        {
            var service = new DataService();
            var products = service.GetProductByCategory(1);
            Assert.Equal(12, products.Count);
            Assert.Equal("Chai", products.First().Name);
            Assert.Equal("Beverages", products.First().Category.Name);
            Assert.Equal("Lakkalikööri", products.Last().Name);
        }
        */
        /* orders */
        /*
        [Fact]
        public void Order_Object_HasIdDatesAndOrderDetails()
        {
            var order = new Order();
            Assert.Equal(0, order.Id);
            Assert.Equal(new DateTime(), order.Date);
            Assert.Equal(new DateTime(), order.Required);
            Assert.Null(order.OrderDetails);
            Assert.Null(order.ShipName);
            Assert.Null(order.ShipCity);
        }

        [Fact]
        public void GetOrder_ValidId_ReturnsCompleteOrder()
        {
            var service = new DataService();
            var order = service.GetOrder(10248);
            Assert.Equal(3, order.OrderDetails.Count);
            Assert.Equal("Queso Cabrales", order.OrderDetails.First().Product.Name);
            Assert.Equal("Dairy Products", order.OrderDetails.First().Product.Category.Name);
        }

        [Fact]
        public void GetOrders()
        {
            var service = new DataService();
            var orders = service.GetOrders();
            Assert.Equal(830, orders.Count);
        }

            */
        /* orderdetails */
        /*
        [Fact]
        public void OrderDetails_Object_HasOrderProductUnitPriceQuantityAndDiscount()
        {
            var orderDetails = new OrderDetails();
            Assert.Equal(0, orderDetails.OrderId);
            Assert.Null(orderDetails.Order);
            Assert.Equal(0, orderDetails.ProductId);
            Assert.Null(orderDetails.Product);
            Assert.Equal(0.0, orderDetails.UnitPrice);
            Assert.Equal(0.0, orderDetails.Quantity);
            Assert.Equal(0.0, orderDetails.Discount);
        }

        [Fact]
        public void GetOrderDetailByOrderId_ValidId_ReturnsProductNameUnitPriceAndQuantity()
        {
            var service = new DataService();
            var orderDetails = service.GetOrderDetailsByOrderId(10248);
            Assert.Equal(3, orderDetails.Count);
            Assert.Equal("Queso Cabrales", orderDetails.First().Product.Name);
            Assert.Equal(14, orderDetails.First().UnitPrice);
            Assert.Equal(12, orderDetails.First().Quantity);
        }

        [Fact]
        public void GetOrderDetailByProductId_ValidId_ReturnsOrderDateUnitPriceAndQuantity()
        {
            var service = new DataService();
            var orderDetails = service.GetOrderDetailsByProductId(11);
            Assert.Equal(38, orderDetails.Count);
            Assert.Equal("1996-07-04", orderDetails.First().Order.Date.ToString("yyyy-MM-dd"));
            Assert.Equal(14, orderDetails.First().UnitPrice);
            Assert.Equal(12, orderDetails.First().Quantity);
        }
        */
    }

}
