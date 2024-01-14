namespace Domain;

public class Account
{
    public Space Space { get; set; }

    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Account name must not be empty");
            }
            _name = value;
        }
    }
    private decimal _amount { get; set; }
    
    public decimal Amount {
        get => _amount;
        set
        {
            if (value < 0)
            {
                throw new DomainException("Your Account total has insufficient funds");
            }
            _amount = value;
        }
    }

    private string _currency { get; set; }
    
    public string Currency {
        get => _currency;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Currency must not be empty");
            }
            _currency = value;
        }
    }

    private DateTime _creationDate { get; set; }
    
    public DateTime CreationDate {
        get => _creationDate;
        set
        {
            if (value > DateTime.Now)
            {
                throw new DomainException("Creation date must be in the past");
            }
            _creationDate = value;
        }
    }
    
    public User User { get; set; }
    
    public int Id { get; set; }

}