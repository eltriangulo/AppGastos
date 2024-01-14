using System.Text.RegularExpressions;
using Domain;
using Repository;

namespace BusinessLogic;

public class LogInLogic
{
    private IRepository<User> _repository;

    public LogInLogic(IRepository<User> userRepository)
    {
        _repository = userRepository;
    }
    public User SearchUser(string email, string password)
    {
        User? foundUser = _repository.Find(x => x.Email == email);
        if(foundUser != null && foundUser.GetPassword() == password)
        {
            return foundUser;
        }
        throw new DomainException("Incorrect email or password");
    }
}