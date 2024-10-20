namespace SharedLib.Data;

using Microsoft.EntityFrameworkCore;
using SharedLib.Models;

public class MyDbContext : DbContext
{
    private const string connectionString = "Server=localhost;Database=BankDB;Integrated Security=True;TrustServerCertificate=True;";
    public DbSet<Card> Cards { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>()
            .HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}