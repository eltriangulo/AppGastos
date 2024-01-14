using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseRepository;

public class AccountDatabaseRepository : IRepository<Account>
{
    private SqlContext _context;
    public AccountDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    public Account? Add(Account? oneElement)
    {
        oneElement.User = _context.Users.Find(oneElement.User.Id);
        oneElement.Space = _context.Spaces.Find(oneElement.Space.Id);
        _context.Accounts.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Account? Find(Func<Account?, bool> filter)
    {
        return _context.Accounts.Where(filter).FirstOrDefault();
    }

    public IList<Account> FindAll()
    {
        return _context.Accounts.Include(a => a.Space).ToList();
    }

    public Account? Update(Account updatedEntity)
    {
        Account foundAccount = _context.Accounts.Find(updatedEntity.Id);
        foundAccount = updatedEntity;
        _context.SaveChanges();
        return foundAccount;
    }

    public void Delete(int id)
    {
        Account foundAccount = _context.Accounts.Find(id);
        _context.Accounts.Remove(foundAccount);
        _context.SaveChanges();
    }
}