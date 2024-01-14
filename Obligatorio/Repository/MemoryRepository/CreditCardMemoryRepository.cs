using Domain;

namespace Repository;

public class CreditCardMemoryRepository : IRepository<CreditCard>
{
    private List<CreditCard> _creditCards = new List<CreditCard>();

    public CreditCard Add(CreditCard creditCard)
    {
        _creditCards.Add(creditCard);
        return creditCard;
    }

    public CreditCard? Find(Func<CreditCard, bool> filter)
    {
        return _creditCards.Where(filter).FirstOrDefault();
    }

    public IList<CreditCard> FindAll()
    {
        return _creditCards;
    }

    public CreditCard Update(CreditCard creditCard)
    {
        CreditCard foundCreditCard = Find(x => x.Last4Digits == creditCard.Last4Digits);
        foundCreditCard = creditCard;
        return foundCreditCard;
    }

    public void Delete(int last4Digits)
    {
        _creditCards.RemoveAll(x => x.Id == last4Digits);
    }
}