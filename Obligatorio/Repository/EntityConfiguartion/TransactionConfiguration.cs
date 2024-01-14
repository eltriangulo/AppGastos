using Microsoft.EntityFrameworkCore;
using Domain;

namespace Repository.EntityConfiguartion;

public class TransactionConfiguration
{
    public static void ConfigureEntity(ModelBuilder modelBuilder)
    {
        

        // modelBuilder.Entity<Transaction>().HasOne(t => t.Exchange)
        //     .WithMany(e => e.Transactions);
    }
}   