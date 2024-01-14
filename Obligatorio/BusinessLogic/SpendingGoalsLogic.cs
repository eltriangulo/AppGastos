using Domain;
using Repository;


namespace BusinessLogic;

public class SpendingGoalsLogic
{
    private readonly IRepository<SpendingGoals> _repository;
    
    public SpendingGoalsLogic(IRepository<SpendingGoals> spendingGoalsRepository)
    {
        _repository = spendingGoalsRepository;
    }
    
    public SpendingGoals AddSpendingGoals(SpendingGoals spendingGoals)
    {
        IsSpendingGoalsTitleUnique(spendingGoals.Title);
        return _repository.Add(spendingGoals);
    }
   
    public void IsSpendingGoalsTitleUnique(string title)
    {
        SpendingGoals? foundSpendingGoals = _repository.Find(x => x.Title == title);
        if (foundSpendingGoals != null)
        {
            throw new DomainException("Spending Goals Title must be unique");
        }
    }

    public void DeleteSpendingGoals(int id)
    {
        SpendingGoals? foundSpendingGoals = _repository.Find(x => x.Id == id);
        if (foundSpendingGoals == null)
        {
            throw new DomainException("Spending Goals not found");
        }
        _repository.Delete(foundSpendingGoals.Id);
    }
    
    public int AssignedId()
    {
        return _repository.FindAll().Count;
    }

    public List<SpendingGoals> GetAllGoalsFromSpace(int currentSpaceId)
    {
        return _repository.FindAll().Where(x => x.Space.Id == currentSpaceId).ToList();
    }
    
    public string ReturnCategories(SpendingGoals goals)
    {
        string categories = "";
        foreach (Category category in goals.Categories)
        {
            categories += category.Name + ", ";
        }
        return categories;
    }
    
    public SpendingGoals GetSpendingGoalsById(int id)
    {
        SpendingGoals? foundSpendingGoals = _repository.Find(x => x.Id == id);
        return foundSpendingGoals;
    }
}