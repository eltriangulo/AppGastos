using Domain;

namespace Repository.DatabaseRepository;

public class UserDatabaseRepository : IRepository<User>
{
    private SqlContext _context;
    public UserDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    public User Add(User oneElement)
    {
        _context.Users.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public User? Find(Func<User, bool> filter)
    {
        return _context.Users.Where(filter).FirstOrDefault();
    }

    public IList<User> FindAll()
    {
        return _context.Users.ToList();
    }

    public User? Update(User updatedEntity)
    {
        User foundUser = _context.Users.Find(updatedEntity.Id);
        foundUser = updatedEntity;
        _context.SaveChanges();
        return foundUser;
    }

    public void Delete(int id)
    {
        User foundUser = _context.Users.Find(id);
        _context.Users.Remove(foundUser);
        _context.SaveChanges();
    }
}