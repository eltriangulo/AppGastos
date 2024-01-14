using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseRepository;

public class CategoryDatabaseRepository : IRepository<Category>
{
    private SqlContext _context;
    public CategoryDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    public Category Add(Category oneElement)
    {
        oneElement.User = _context.Users.Find(oneElement.User.Id);
        oneElement.Space = _context.Spaces.Find(oneElement.Space.Id);
        _context.Categories.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Category? Find(Func<Category, bool> filter)
    {
        return _context.Categories.
            Include(c =>c.Space)
            .Where(filter).FirstOrDefault();
    }

    public IList<Category> FindAll()
    {
        return _context.Categories.Include(c => c.Space).ToList();
    }

    public Category? Update(Category updatedEntity)
    {
        Category foundCategory = _context.Categories.Find(updatedEntity.Id);
        foundCategory = updatedEntity;
        _context.SaveChanges();
        return foundCategory;
    }

    public void Delete(int id)
    {
        Category foundCategory = _context.Categories.Find(id);
        _context.Categories.Remove(foundCategory);
        _context.SaveChanges();
    }
}