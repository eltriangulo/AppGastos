using Domain;

namespace Repository;

public class TransactionMemoryRepository : IRepository<Transaction>
{
    private readonly IList<Transaction> _transactions = new List<Transaction>();

    public Transaction Add(Transaction oneElement)
    {
        _transactions.Add(oneElement);
        return oneElement;
    }

    public Transaction? Find(Func<Transaction, bool> filter)
    {
        return _transactions.FirstOrDefault(filter);
    }

    public IList<Transaction> FindAll()
    {
        return _transactions;
    }

    public Transaction? Update(Transaction updatedEntity)
    {
        var transaction = Find(t => t.Id == updatedEntity.Id);
        if (transaction == null) return null;
        transaction.Title = updatedEntity.Title;
        transaction.Date = updatedEntity.Date;
        transaction.Amount = updatedEntity.Amount;
        transaction.Currency = updatedEntity.Currency;
        transaction.Category = updatedEntity.Category;
        return transaction;
    }

    public void Delete(int Transid)
    {
        _transactions.Remove(Find(t => t.Id == Transid));
    }
    
}