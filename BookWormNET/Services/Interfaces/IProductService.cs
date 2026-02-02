using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddAsync(Product product);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> UpdateAsync(int id, Product Updatedproduct);
        Task<bool> DeleteAsync(int id);

    }
}
