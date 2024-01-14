using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityConfiguartion;

public class SpendingGoalsConfiguration
{
    public static void ConfigureEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SpendingGoals>().HasMany(s => s.Categories)
            .WithMany(c => c.SpendingGoals);

    }
}