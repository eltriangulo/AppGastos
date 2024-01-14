namespace Domain;

public class Transaction
{
    private string _title { get; set; }
    
    private decimal _amount { get; set; }
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Currency { get; set; }
    public Category Category { get; set; }
    public Account? Account { get; set; }
    
    public CreditCard? CreditCard { get; set; }
    
    public Exchange? Exchange { get; set; }
    
    public Space Space { get; set; }

    public string Title
    {
        get => _title;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Title must not be empty");
            }
            _title = value;
        }
    }
    
    public decimal Amount
    {
        get => _amount;
        set
        {
            if (value < 1)
            {
                throw new DomainException("Amount must be greater than 0");
            }
            _amount = value;
        }
    }
   

}