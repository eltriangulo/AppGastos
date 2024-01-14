namespace Domain;

public class Space
{
    
    public int Id { get; set; }
    
    private string _name { get; set; }
    
    public string Name {
        get => _name;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Name must not be empty");
            }
            _name = value;
        }
    }
    
    public User Admin { get; set; }
    
    public List<User> InvitedUsers { get; set; } = new List<User>();
    
    
}