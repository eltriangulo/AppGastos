using Domain;
using Repository;

namespace BusinessLogic;

public class AccountLogic
{
    private readonly IRepository<Account?> _repository;

    public AccountLogic(IRepository<Account?> accountRepository)
    {
        _repository = accountRepository;
    }

    public Account? AddAccount(Account? account)
    {
        if (IsRepeatedNameForSpace(account.Space.Id, account.Name))
        {
            throw new LogicException("Can't add Account with same name");
        }
        return _repository.Add(account);
    }

    public List<Account?> GetAllAccountsFromSpace(int spaceId)
    {
        return _repository.FindAll().Where(x => x.Space.Id == spaceId).ToList();
    }
    
    public Account? GetAccountFromSpaceByName(string name, int id)
    {
        return _repository.Find(x => x.Name == name && x.Space.Id == id);
    }

    public bool IsRepeatedNameForSpace(int spaceId, string name)
    {
        return _repository.FindAll().Any(x => x.Space.Id == spaceId && x.Name == name);
    }
    
    public void DeleteAccount(int account)
    {
        _repository.Delete(account);
    }

    public void UpdateAccount(Account? account)
    {
        _repository.Update(account);
    }
    
    public void UpdateAccountAmount(Account? account, decimal amount, Category category)
    {
        if (category.Type == "Income")
        {
            account.Amount += amount;
        }
        else if (category.Type == "Expenses")
        {
            account.Amount -= amount;
        }
        _repository.Update(account);}
}