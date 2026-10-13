using Microsoft.EntityFrameworkCore;
using OwlBank.Models;
using System.Text.Json;
using OwlBank.DTOs.UserDTO;

namespace OwlBank.Repository;

public class OwlBankDBContext(DbContextOptions<OwlBankDBContext> option) : DbContext(option)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<BankStatement> BankStatement { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Version)
            .IsRowVersion();

        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.LastName, u.FirstName });
    }
}