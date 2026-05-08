using Microsoft.EntityFrameworkCore;
using OwlBank.Models;

namespace OwlBank.Repository;

public class OwlBankDBContext : DbContext
{
    public OwlBankDBContext(DbContextOptions<OwlBankDBContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<BankStatement> BankStatement { get; set; }
}