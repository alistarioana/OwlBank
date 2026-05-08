using Microsoft.EntityFrameworkCore;
using OwlBank.Models;
using System.Text.Json;

namespace OwlBank.Repository;

public class OwlBankDBContext : DbContext
{
    public OwlBankDBContext(DbContextOptions<OwlBankDBContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<BankStatement> BankStatement { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}