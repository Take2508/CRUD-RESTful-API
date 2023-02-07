using FastDeliveryApi.Data.Configuration;
using FastDeliveryApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace FastDeliveryApi.Data;

public class FastDeliveryDbContext : DbContext 
{
     public FastDeliveryDbContext(DbContextOptions options ) : base(options)
    {
        
    }
    public DbSet <Customer> Customers  { get; set; }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
   
   {
      base.OnModelCreating (modelBuilder);
      modelBuilder.ApplyConfiguration(new CustomerConfiguration());
   }

    internal object GetcustomerById(int id)
    {
        throw new NotImplementedException();
    }

    internal Task ModifyAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}