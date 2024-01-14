using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseRepository;

public class SpaceDatabaseRepository : IRepository<Space>
{
    private SqlContext _context;
    public SpaceDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    public Space Add(Space oneElement)
    {
        oneElement.Admin = _context.Users.Find(oneElement.Admin.Id);
        _context.Spaces.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Space? Find(Func<Space, bool> filter)
    {
        return _context.Spaces.Where(filter).FirstOrDefault();
    }

    public IList<Space> FindAll()
    {
        return _context.Spaces.Include(s => s.Admin).Include(s => s.InvitedUsers).ToList();
    }

    public Space? Update(Space updatedEntity)
    {
        Space foundSpace = _context.Spaces.Find(updatedEntity.Id);
        foundSpace = updatedEntity;
        _context.SaveChanges();
        return foundSpace;
    }

    public void Delete(int id)
    {
        Space foundSpace = _context.Spaces.Find(id);
        _context.Spaces.Remove(foundSpace);
        _context.SaveChanges();
    }
}