using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseRepository;

public class TransactionDatabaseRepository : IRepository<Transaction>
{
    
    private SqlContext _context;
    public TransactionDatabaseRepository(SqlContext context)
    {
        _context = context;
    }
    
    public Transaction Add(Transaction oneElement){
        
        if (oneElement.Account != null)
        {
            oneElement.Account = _context.Accounts.Find(oneElement.Account.Id) ?? throw new InvalidOperationException("Account not found");
        }
        if (oneElement.CreditCard != null)
        {
            oneElement.CreditCard = _context.CreditCards.Find(oneElement.CreditCard.Id) ?? throw new InvalidOperationException("CreditCard not found");
        }
        if (oneElement.Exchange != null)
        {
            oneElement.Exchange = _context.Exchanges.Find(oneElement.Exchange.Id) ?? throw new InvalidOperationException("Exchange not found");
        }

        oneElement.Space = _context.Spaces.Find(oneElement.Space.Id);
        oneElement.Category = _context.Categories.Find(oneElement.Category.Id) ?? throw new InvalidOperationException("Category not found");

        _context.Transactions.Add(oneElement);
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            // Log or inspect innerException for more details
            throw;
        }

        return oneElement;
    }
    
    public Transaction? Find(Func<Transaction, bool> filter)
    {
        return _context.Transactions.Where(filter).FirstOrDefault();
    }
    
    public IList<Transaction> FindAll()
    {
        return _context.Transactions
            .Include(t => t.Space)
            .Include(t => t.Account)
            .Include(t=> t.CreditCard)
            .Include(t => t.Exchange)
            .Include(t => t.Category)
            .ToList();
    }
    
    public Transaction? Update(Transaction updatedEntity)
    {
        Transaction foundTransaction = _context.Transactions.Find(updatedEntity.Id);
        if (foundTransaction != null)
        {
            _context.Entry(foundTransaction).CurrentValues.SetValues(updatedEntity);
            _context.SaveChanges();
        }
        return foundTransaction;
    }
    
    public void Delete(int id)
    {
        Transaction foundTransaction = _context.Transactions.Find(id);
        _context.Transactions.Remove(foundTransaction);
        _context.SaveChanges();
    }
    
    
}