using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> AddProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<List<Product>?> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return null;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return await _context.Products.ToListAsync();
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var products = await _context.Products.ToListAsync();
        return products;
    }

    public async Task<Product?> GetSingleProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return null;

        return product;
    }

    public async Task<List<Product>?> UpdateProduct(int id, Product request)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return null;

        product.Name = request.Name;
        product.Price = request.Price;

        await _context.SaveChangesAsync();

        return await _context.Products.ToListAsync();
    }
}
