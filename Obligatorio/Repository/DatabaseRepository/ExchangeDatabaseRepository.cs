using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseRepository;

public class ExchangeDatabaseRepository : IRepository<Exchange>
{
    private SqlContext _context;
    public ExchangeDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    
    public Exchange? Add(Exchange? oneElement)
    {
        oneElement.Space = _context.Spaces.Find(oneElement.Space.Id);
        _context.Exchanges.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }
    
    public Exchange? Find(Func<Exchange?, bool> filter)
    {
        return _context.Exchanges.Where(filter).FirstOrDefault();
    }
    
    public IList<Exchange> FindAll()
    {
        return _context.Exchanges.Include(x => x.Space).ToList();
    }
    
    public Exchange? Update(Exchange updatedEntity)
    {
        Exchange foundExchange = _context.Exchanges.Find(updatedEntity.Id);
        foundExchange = updatedEntity;
        _context.SaveChanges();
        return foundExchange;
    }
    
    public void Delete(int id)
    {
        Exchange foundExchange = _context.Exchanges.Find(id);
        _context.Exchanges.Remove(foundExchange);
        _context.SaveChanges();
    }
 
}