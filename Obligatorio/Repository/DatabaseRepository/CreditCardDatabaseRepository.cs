using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseRepository;

public class CreditCardDatabaseRepository : IRepository<CreditCard>
{
    private SqlContext _context;
    public CreditCardDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    public CreditCard? Add(CreditCard? oneElement)
    {
        oneElement.User = _context.Users.Find(oneElement.User.Id);
        oneElement.Space = _context.Spaces.Find(oneElement.Space.Id);
        _context.CreditCards.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public CreditCard? Find(Func<CreditCard?, bool> filter)
    {
        return _context.CreditCards.Where(filter).FirstOrDefault();
    }

    public IList<CreditCard?> FindAll()
    {
        return _context.CreditCards.Include(cc => cc.Space).ToList();
    }

    public CreditCard? Update(CreditCard updatedEntity)
    {
        CreditCard foundCreditCard = _context.CreditCards.Find(updatedEntity.Id);
        foundCreditCard = updatedEntity;
        _context.SaveChanges();
        return foundCreditCard;
    }

    public void Delete(int id)
    {
        CreditCard foundCreditCard = _context.CreditCards.Find(id);
        _context.CreditCards.Remove(foundCreditCard);
        _context.SaveChanges();
    }
}