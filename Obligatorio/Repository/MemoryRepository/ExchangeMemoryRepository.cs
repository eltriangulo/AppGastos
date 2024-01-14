using Domain;

namespace Repository;

public class ExchangeMemoryRepository : IRepository<Exchange>
{
    private List<Exchange> _exchanges = new List<Exchange>();


    public Exchange Add(Exchange oneElement)
    {
        _exchanges.Add(oneElement);
        return oneElement;
    }

    public Exchange? Find(Func<Exchange, bool> filter)
    {
        return _exchanges.Where(filter).FirstOrDefault();
    }

    public IList<Exchange> FindAll()
    {
        return _exchanges;
    }

    public Exchange? Update(Exchange updatedEntity)
    {
        Exchange foundExchange = Find(x => x.Date == updatedEntity.Date);
        foundExchange = updatedEntity;
        return foundExchange;
    }

    public void Delete(int id)
    {
        _exchanges.Remove(Find(x => x.Id == id));
    }
}