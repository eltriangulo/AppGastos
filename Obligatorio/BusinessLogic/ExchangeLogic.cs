using Domain;
using Repository;

namespace BusinessLogic;

public class ExchangeLogic
{
    private readonly IRepository<Exchange?> _repository;
    
    public ExchangeLogic(IRepository<Exchange?> exchangeRepository)
    {
        _repository = exchangeRepository;
    }
    
    public Exchange? AddExchange(Exchange? exchange)
    {
        if (exchange.Date == null || exchange.Date == default)
        {
            throw new DomainException("Date is null");
        }
        return _repository.Add(exchange);
    }

    public void DeleteExchange(int exchangeId)
    {
        _repository.Delete(exchangeId);
    }

    public List<Exchange?> GetExchangesFromSpace(int currentSpaceId)
    {
        return _repository.FindAll().Where(x => x.Space.Id == currentSpaceId).ToList();
    }
    
    public Exchange? FindExchangeByDateFromSpace(DateTime date, int spaceId, string currency)
    {
        return _repository.Find(x => x.Date.Day == date.Day && x.Date.Month == date.Month &&
                                     x.Date.Year == date.Year && x.Space.Id == spaceId && x.Currency == currency);
    }
    public Exchange FindExchangeById(int exchangeId)
    {
        return _repository.Find(x => x.Id == exchangeId);
    }

    public void UpdateExchange(Exchange? exchange)
    {
        _repository.Update(exchange);
    }
}