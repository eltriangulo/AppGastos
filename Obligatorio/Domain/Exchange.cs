namespace Domain;

public class Exchange
{
    private decimal _value { get; set; }
    
    public int Id { get; set; }
    public Space Space { get; set; }
    
    public string Currency { get; set; }
    
    public decimal Value
    {
        get => _value;
        set
        {
            if (value <= 0)
            {
                throw new DomainException("USD value must be greater than 0");
            }
            _value = value;
        }
    }


    public DateTime Date { get; set; }
    
    

}