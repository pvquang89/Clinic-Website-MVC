using Microsoft.EntityFrameworkCore;
using WebPhongKham.Models;
namespace WebPhongKham.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProvinceType> ProvinceTypes { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<News> News { get; set; }



    }
}
