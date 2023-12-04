using Domain.Models;

namespace Domain.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();
    Task<Product?> GetSingleProduct(int id);
    Task<List<Product>?> AddProduct(Product product);
    Task<List<Product>?> UpdateProduct(int id, Product product);
    Task<List<Product>?> DeleteProduct(int id);
}
