using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.EntityConfiguartion;

namespace Repository;

public class SqlContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Space> Spaces { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    
    public DbSet<CreditCard?> CreditCards { get; set; }
    
    public DbSet<Account?> Accounts { get; set; }
    
    public DbSet<Exchange?> Exchanges { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    
    public DbSet<SpendingGoals> SpendingGoals { get; set; }
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate(); // Ejecuta las migraciones al cread el DB
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserConfiguration.ConfigureEntity(modelBuilder);
        ExchangeConfiguration.ConfigureEntity(modelBuilder);
        //CategoryConfiguration.ConfigureEntity(modelBuilder);
        SpendingGoalsConfiguration.ConfigureEntity(modelBuilder);
        //SpaceConfiguration.ConfigureEntity(modelBuilder);
    }
    
    
}