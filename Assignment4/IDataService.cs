using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IDataService
    {

        List<Question> GetPostsByName(string name, int page, int pageSize, out int totalResults);
        Question GetPost(int id);

        Question GetQuestion(int id);
        /*
Product GetProduct(int id);

List<Product> GetProducts();

List<Product> GetProductByCategory(int id);

List<Product> GetProductByName(string name);

Category GetCategory(int id);

List<Category> GetCategories();

Category CreateCategory(string name, string description);

Boolean UpdateCategory(int id, string name, string description);

Boolean DeleteCategory(int id);
*/
    }
}
