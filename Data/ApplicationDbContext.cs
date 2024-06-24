using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users {get;set;}
    public DbSet<Admin> Admins {get;set;}
    public DbSet<BramProduct> Products {get;set;}
    public DbSet<Cart> Carts {get;set;}
    public DbSet<Transaction> Transactions {get;set;}
    public DbSet<TransactionDetail> TransactionDetails {get;set;}

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base (options) { }
}