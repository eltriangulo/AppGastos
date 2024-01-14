using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Repository.EntityConfiguartion;

public class UserConfiguration
{
    public static void ConfigureEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Name).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Surname).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        modelBuilder.Entity<User>().Property(u => u._password).IsRequired();
     
        modelBuilder.Entity<User>().HasMany(x => x._spaces)
            .WithMany(x => x.InvitedUsers);
        
        modelBuilder.Entity<User>().HasMany(x => x._administeredSpaces)
            .WithOne(x => x.Admin);


    }
}