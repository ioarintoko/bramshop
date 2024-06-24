using System;
using System.Linq;
using Data;
using Models;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Admins.Any())
        {
            _context.Admins.AddRange(
                new Admin { Name = "Bram1", Email = "Bram1@shop.com", Password = "admin123" },
                new Admin { Name = "Bram2", Email = "Bram2@shop.com", Password = "admin456" }
                // Add more admins as needed
            );
        }

        if (!_context.Products.Any())
        {
            _context.Products.AddRange(
                new BramProduct { Name = "Sepatu", Stock = 100, Picture = "product1.jpg", Specification = "Spec for Product1" },
                new BramProduct { Name = "Kaus Kaki", Stock = 150, Picture = "product2.jpg", Specification = "Spec for Product2" }
                // Add more products as needed
            );
        }

        if (!_context.Users.Any())
        {
            _context.Users.AddRange(
                new User { Name = "BramUser1", Email = "user1@example.com", Password = "user123", Address = "Address1", Phone = "123456789" },
                new User { Name = "BramUser2", Email = "user2@example.com", Password = "user456", Address = "Address2", Phone = "987654321" }
                // Add more users as needed
            );
        }
        
         if (!_context.Carts.Any())
            {
                _context.Carts.AddRange(
                    new Cart { IdProduct = 1, Quantity = 2, Price = 20 },
                    new Cart { IdProduct = 2, Quantity = 3, Price = 30 }
                    // Add more cart items as needed
                );

                _context.SaveChanges();
            }

         if (!_context.Transactions.Any())
            {
                _context.Transactions.AddRange(
                    new Transaction { IdUser = 1, CreateDate = DateTime.Now, SendDate = DateTime.Now.AddDays(1), Status = "Pending", Receipt = "ABC123", Total = 50 },
                    new Transaction { IdUser = 2, CreateDate = DateTime.Now, SendDate = DateTime.Now.AddDays(2), Status = "Completed", Receipt = "DEF456", Total = 75 }
                    // Add more transactions as needed
                );

                _context.SaveChanges();
            }

         if (!_context.TransactionDetails.Any())
            {
                _context.TransactionDetails.AddRange(
                    new TransactionDetail { IdProduct = 1, Quantity = 2, Price = 20 },
                    new TransactionDetail { IdProduct = 2, Quantity = 3, Price = 30 }
                    // Add more transaction details as needed
                );

                _context.SaveChanges();
            }

        _context.SaveChanges();
    }
}
