using Microsoft.EntityFrameworkCore;
using Repository;

namespace BusinessLogicTest.Context;

public class SqlContextFactory
{
    public SqlContext CreateMemoryContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();
        optionsBuilder.UseInMemoryDatabase("TestDB");

        return new SqlContext(optionsBuilder.Options);
    }
}