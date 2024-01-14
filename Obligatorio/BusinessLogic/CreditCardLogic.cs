using Domain;
using Repository;

namespace BusinessLogic;

public class CreditCardLogic
{
    private readonly IRepository<CreditCard?> _repository;
    
    public CreditCardLogic(IRepository<CreditCard?> creditCardRepository)
    {
        _repository = creditCardRepository;
    }
        
    public CreditCard? AddCreditCard(CreditCard? creditCard)
    {
        return _repository.Add(creditCard);
    }
    public List<CreditCard?> GetAllCreditCardsFromSpace(int idSpace)
    {
        return _repository.FindAll().Where(x => x.Space.Id == idSpace).ToList();
    }
    
    public void DeleteCreditCard(int CreditCardId)
    {
        _repository.Delete(CreditCardId);
    }

    public CreditCard? GetCreditCard(int idSpace, string last4)
    {
        return _repository.Find(x => x.Space.Id == idSpace && x.Last4Digits == last4);
    }

    public void UpdateCreditCard(CreditCard? creditCard)
    {
        _repository.Update(creditCard);
    }

    public void UpdateCreditCardAmount(CreditCard? transactionCreditCard, decimal amount, Category transactionCategory)
    {
        if (transactionCategory.Type == "Income")
        {
            transactionCreditCard.AvaiableCredit += amount;
        }
        else if (transactionCategory.Type == "Expenses")
        {
            transactionCreditCard.AvaiableCredit -= amount;
        }
        _repository.Update(transactionCreditCard);
    }
}