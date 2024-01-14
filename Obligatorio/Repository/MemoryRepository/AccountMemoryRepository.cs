using Domain;

namespace Repository;

public class AccountMemoryRepository : IRepository<Account>
{
    public List<Account> _accounts = new List<Account>();
    
    public Account Add(Account oneElement)
    {
        _accounts.Add(oneElement);
        return oneElement;
    }
    
    public Account? Find(Func<Account, bool> filter)
    {
        return _accounts.Where(filter).FirstOrDefault();
    }
    
    public IList<Account> FindAll()
    {
        return _accounts;
    }
    
    public Account? Update(Account updatedEntity)
    {
        Account foundAccount = Find(x => x.Name == updatedEntity.Name);
        foundAccount = updatedEntity;
        return foundAccount;
    }
    
    public void Delete(int AccountId)
    {
        _accounts.RemoveAll(x => x.Id == AccountId);
    }
    
    
    
}