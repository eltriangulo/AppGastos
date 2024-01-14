namespace Domain;

public class User
{
    private string _name;
    
    public string Name
    {
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
    
    private string _surname;
    
    public string Surname
    {
        get => _surname;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Surname must not be empty");
            }
            _surname = value;
        }
    }
    
    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("Email must not be empty");
            }
            _email = value;
        }
    }

    private string _address;
    
    public string Address
    {
        get => _address;
        set => _address = value;
    }

    public string _password { get; set; }
    
    public void SetPassword(string password)
    {
        if (password is null || password.Length == 0)
        {
            throw new DomainException("Password must not be empty");
        }
        _password = password;
    }
    
    private int _id;
    
    public int Id
    {
        get => _id;
        set => _id = value;
    }
    
    public List<Space> _spaces = new List<Space>();
    
    public List<Space> _administeredSpaces = new List<Space>();
    
    public string GetPassword()
    {
        return _password;
    }
    
    public override string ToString()
    {
        return Name + " " + Surname;
    }
}