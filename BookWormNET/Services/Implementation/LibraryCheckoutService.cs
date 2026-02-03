using BookWormNET.Data;
using BookWormNET.DTOs;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using System;
using System.Linq;

namespace BookWormNET.Services.Implementation
{
    public class LibraryCheckoutService : ILibraryCheckoutService
    {
        private readonly BookwormDbContext _context;

        public LibraryCheckoutService(BookwormDbContext context)
        {
            _context = context;
        }

        public void Checkout(LibraryCheckoutRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // 1️⃣ User
                var user = _context.Users
                    .FirstOrDefault(u => u.UserId == request.UserId)
                    ?? throw new Exception("User not found");

                // 2️⃣ Library Package
                var package = _context.LibraryPackages
                    .FirstOrDefault(p => p.PackageId == request.PackageId)
                    ?? throw new Exception("Library package not found");

                // 3️⃣ Active borrowed books (FIXED)
                int activeBorrowedBooks = _context.MyLibraries
                    .Count(m =>
                        m.UserId == user.UserId &&
                        m.EndDate > DateOnly.FromDateTime(DateTime.UtcNow));

                int selectedBooks = request.ProductIds.Count;

                // 4️⃣ Book limit
                if (activeBorrowedBooks + selectedBooks > package.BookLimit)
                    throw new Exception("Library book limit exceeded");

                // 5️⃣ Dates (correct for DateOnly)
                var startDate = DateOnly.FromDateTime(DateTime.UtcNow);
                var endDate = startDate.AddDays(package.ValidityDays ?? 0);

                // 6️⃣ Create MyLibrary per book
                foreach (var productId in request.ProductIds)
                {
                    var product = _context.Products
                        .FirstOrDefault(p => p.ProductId == productId)
                        ?? throw new Exception("Product not found");

                    if (product.IsLibrary != true)
                    {
                        throw new Exception(
                            $"Product {product.ProductName} is not available for library");
                    }


                    var myLibrary = new MyLibrary
                    {
                        UserId = user.UserId,
                        PackageId = package.PackageId,
                        ProductId = product.ProductId,

                        User = user,
                        Package = package,
                        Product = product,

                        StartDate = startDate,
                        EndDate = endDate,

                        BooksAllowed = package.BookLimit ?? 0,
                        BooksTaken = activeBorrowedBooks + selectedBooks
                    };

                    _context.MyLibraries.Add(myLibrary);
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
