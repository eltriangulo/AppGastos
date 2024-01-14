namespace Domain;

public class CreditCard
{
    public int Id { get; set; }
    private string _issuingBank { get; set; }
    
    public string IssuingBank
    {
        get => _issuingBank;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Issuing bank must not be empty");
            }
            _issuingBank = value;
        }
    }
    
    private string _last4Digits { get; set; }
    
    public string Last4Digits
    {
        get => _last4Digits;
        set
        {
            if(value.Length != 4)
            {
                throw new DomainException("Last 4 digits must be 4 digits");
            }
            _last4Digits = value;
        }
    }
    
    public string Currency { get; set; }
    
    public decimal AvaiableCredit { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime DeadlineDate { get; set; }
    
    public User User { get; set; }
    
    public Space Space { get; set; }
    
}