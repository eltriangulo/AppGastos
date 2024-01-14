using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityConfiguartion;

public class SpaceConfiguration
{
    public static void ConfigureEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Space>().HasKey(s => s.Id);
        modelBuilder.Entity<Space>().Property(s => s.Name).IsRequired();
        // modelBuilder.Entity<Space>().HasMany(x => x._categories)
        //     .WithOne(x => x.Space);
        modelBuilder.Entity<Space>().HasMany(x => x.InvitedUsers)
            .WithMany(x => x._spaces);
        // modelBuilder.Entity<Space>().HasMany(x => x._administeredCategories)
        //     .WithOne(x => x.Admin);
   
    }
    
}