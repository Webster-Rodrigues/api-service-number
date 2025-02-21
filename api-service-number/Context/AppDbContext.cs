using api_service_number.Models;
using api_service_number.Models.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace api_service_number.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    { }
    
    public DbSet<Ticket>? Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Ticket>().Property(p => p.Priority).HasConversion(c => c.ToString(),
            c => (Priority)Enum.Parse(typeof(Priority), c));
        
        modelBuilder.Entity<Ticket>().Property(p => p.Status).HasConversion(c => c.ToString(),
            c => (Status)Enum.Parse(typeof(Status), c));
    }
    
}