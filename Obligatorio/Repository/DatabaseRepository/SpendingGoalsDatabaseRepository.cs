using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseRepository;

public class SpendingGoalsDatabaseRepository : IRepository<SpendingGoals>
{
    private SqlContext _context;
    public SpendingGoalsDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    public SpendingGoals Add(SpendingGoals oneElement)
    {
        oneElement.Space = _context.Spaces.Find(oneElement.Space.Id);
        _context.SpendingGoals.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public SpendingGoals? Find(Func<SpendingGoals, bool> filter)
    {
        return _context.SpendingGoals.Where(filter).FirstOrDefault();
    }

    public IList<SpendingGoals> FindAll()
    {
        return _context.SpendingGoals.Include(s => s.Space).ToList();
    }

    public SpendingGoals? Update(SpendingGoals updatedEntity)
    {
        SpendingGoals foundSpendingGoals = _context.SpendingGoals.Find(updatedEntity.Id);
        foundSpendingGoals = updatedEntity;
        _context.SaveChanges();
        return foundSpendingGoals;
    }

    public void Delete(int id)
    {
        SpendingGoals foundSpendingGoals = _context.SpendingGoals.Find(id);
        _context.SpendingGoals.Remove(foundSpendingGoals);
        _context.SaveChanges();
    }
    
}