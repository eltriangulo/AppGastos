namespace Domain;

public class SpendingGoals
{
    public int Id { get; set; }

    public bool isShared { get; set; }
    public Space Space { get; set; }
    private string title { get; set; }
    
    public string Title
    {
        get => title;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Title must not be empty");
            }
            title = value;
        }
    }

    private decimal monthlyBudget { get; set; }
    
    public decimal MonthlyBudget
    {
        get => monthlyBudget;
        set
        {
            if (value < 0)
            {
                throw new DomainException("Monthly Budget must be greater than 0");
            }
            monthlyBudget = value;
        }
    }


    public List<Category> Categories { get; set; } = new List<Category>();
    
    
    
}