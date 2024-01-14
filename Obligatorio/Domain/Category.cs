namespace Domain;

public class Category
{
    private string _name;

    private int _id;

    public List<SpendingGoals>? SpendingGoals = new List<SpendingGoals>();

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public Space Space { get; set; }
    
    public string Name
    {
        get => _name;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Category name must not be empty");
            }
            _name = value;
        }
    }
    
    
    private DateTime _creationDate;
    
    public DateTime CreationDate
    {
        get => _creationDate;
        set =>
            _creationDate = value;
        
    }
    
    private string _status;
    
    public string Status
    {
        get => _status;
        set
        {
            if (value is null || value.Length == 0 )
            {
                throw new DomainException("Status must not be empty");
            }
            _status = value;
        }
    }
    
    private string _type;
    
    public string Type
    {
        get => _type;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Type must not be empty");
            }

            _type = value;
        }
    }
    
    private User _user;
    
    public User User
    {
        get => _user;
        set => _user = value;
    }
    
    public bool IsActiveCategory()
    {
        return Status.Equals("Active");
    }
}