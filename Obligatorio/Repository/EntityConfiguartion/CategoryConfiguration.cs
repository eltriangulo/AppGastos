using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityConfiguartion;

public class CategoryConfiguration
{
    public static void ConfigureEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasMany(s => s.SpendingGoals)
            .WithMany(c => c.Categories);
    }
}