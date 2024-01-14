using Domain;

namespace Repository;

public class SpendingGoalsMemoryRepository : IRepository<SpendingGoals>
{
    private readonly List<SpendingGoals> _spendingGoals = new List<SpendingGoals>();
    
       public SpendingGoals Add(SpendingGoals oneElement)
        {
            _spendingGoals.Add(oneElement);
            return oneElement;
        }
       
       public SpendingGoals? Find(Func<SpendingGoals, bool> filter)
       {
           return _spendingGoals.FirstOrDefault(filter);
       }
       
       public IList<SpendingGoals> FindAll()
       {
           return _spendingGoals;
       }
       
       public SpendingGoals? Update(SpendingGoals updatedEntity)
       {
           SpendingGoals foundUser = Find(x => x.Title == updatedEntity.Title);
           foundUser = updatedEntity;
           return foundUser;
       }
       
       public void Delete(int id)
       {
           _spendingGoals.RemoveAll(x => x.Id == id);
       }

       
}