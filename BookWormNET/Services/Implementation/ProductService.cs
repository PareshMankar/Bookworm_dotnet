using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly BookwormDbContext _context;

        public ProductService(BookwormDbContext context)
        {
            _context = context;
        }


        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            //return await _context.Products
            //.Include(p => p.ProductAuthorNavigation)
            //.Include(p => p.ProductGenereNavigation)
            //.Include(p => p.ProductPublisherNavigation)
            //.Include(p => p.ProductLangNavigation)
            //.ToListAsync();
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.ProductAuthorNavigation)
            .Include(p => p.ProductGenereNavigation)
            .Include(p => p.ProductPublisherNavigation)
            .Include(p => p.ProductLangNavigation)
            .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public  async Task<Product?> UpdateAsync(int id, Product Updatedproduct)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(Updatedproduct);
            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
