using Domain;
using Repository;

namespace BusinessLogic;

public class UserLogic
{
    private readonly IRepository<User> _repository;

    public UserLogic(IRepository<User> userRepository)
    {
        _repository = userRepository;
    }

    public User AddUser(User user)
    {
        CheckRepeatedEmail(user);
        ValidateEmailFormat(user.Email);
        _repository.Add(user);
        return user;
    }

    public void CheckRepeatedEmail(User user)
    {
        User? foundUser = _repository.Find(x => x.Email == user.Email);
        if (foundUser != null)
        {
            throw new LogicException("Can't add user with same email");
        }
    }


    public bool PasswordFormatOk(string contrasena)
    {
        if (contrasena is null || contrasena.Length == 0)
        {
            throw new LogicException("Password must not be empty");
        }
        if (contrasena.Length > 9 && contrasena.Length < 31 && contrasena.Any(char.IsUpper))
        {
            return true;
        }
        throw new LogicException(
            "Password must be between 10 and 30 characters and contain at least one uppercase letter");
    }

    public void PasswordAreTheSame(string password, string currentUserPassword)
    {
        if (password != currentUserPassword)
        {
            throw new LogicException("Passwords are not the same");
        }
    }

    public void ValidateEmailFormat(string Email)
    {
        if(Email is null || Email.Length == 0)
        {
            throw new LogicException("Email must not be empty");
        }
        if (Email.Contains('@') == false || Email.EndsWith(".com", StringComparison.OrdinalIgnoreCase) == false)
        {
            throw new LogicException("Email must contain @ and end with .com");
        }
    }

    public User GetUserByEmail(string Email)
    {
        User? foundUser = _repository.Find(x => x.Email == Email);
        if (foundUser == null)
        {
            throw new LogicException("User not found");
        }
        return foundUser;
    }
    
    public void UpdateUser(User user)
    {
        _repository.Update(user);
    }
}
