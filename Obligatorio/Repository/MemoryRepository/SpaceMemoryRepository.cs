using Domain;

namespace Repository;

public class SpaceMemoryRepository : IRepository<Space>
{
    private List<Space> _spaces = new List<Space>();
    
    public Space Add(Space oneElement)
    {
        _spaces.Add(oneElement);
        return oneElement;
    }
    
    public Space? Find(Func<Space, bool> filter)
    {
        return _spaces.Where(filter).FirstOrDefault();
    }
    
    public IList<Space> FindAll()
    {
        return _spaces;
    }
    
public Space? Update(Space updatedEntity)
    {
        Space foundSpace = Find(x => x.Id == updatedEntity.Id);
        foundSpace = updatedEntity;
        return foundSpace;
    }

    public void Delete(int id)
    {
        _spaces.RemoveAll(x => x.Id == id);
    }
    
}