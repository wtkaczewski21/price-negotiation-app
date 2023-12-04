using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
                return NotFound("Product not found");

            return Ok(dbProduct);
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(await _context.Products.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Product>>> UpdateProduct(Product updatedProduct)
        {
            var dbProduct = await _context.Products.FindAsync(updatedProduct.Id);
            if (dbProduct == null)
                return NotFound("Product not found");

            dbProduct.Name = updatedProduct.Name;
            dbProduct.Description = updatedProduct.Description;
            dbProduct.Price = updatedProduct.Price;
            await _context.SaveChangesAsync();

            return Ok(await _context.Products.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
                return NotFound("Product not found");

            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();

            return Ok(await _context.Products.ToListAsync());
        }
    }
}
