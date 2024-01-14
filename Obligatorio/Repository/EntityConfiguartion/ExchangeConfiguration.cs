using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityConfiguartion;

public class ExchangeConfiguration
{
    public static void ConfigureEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exchange>().Property(e => e.Value).IsRequired().HasColumnType("decimal(4,2)");
    }
}