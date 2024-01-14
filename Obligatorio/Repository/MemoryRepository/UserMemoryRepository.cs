using Domain;

namespace Repository;

public class UserMemoryRepository : IRepository<User>
{
    private List<User> _users = new List<User>();
    //private List<Category> _categories = new List<Category>();
    public User Add(User oneElement)
    {
        _users.Add(oneElement);
        return oneElement;
    }

    public User? Find(Func<User, bool> filter)
    {
        return _users.Where(filter).FirstOrDefault();
    }

    public IList<User> FindAll()
    {
        return _users;
    }

    public User? Update(User updatedEntity)
    {
       User foundUser = Find(x => x.Email == updatedEntity.Email);
       foundUser = updatedEntity;
       return foundUser;
    }

    public void Delete(int Id)
    {
        _users.RemoveAll(x => x.Id == Id);
    }

    private User userExample = new User
    {
        Name = "Franco",
        Surname = "Caceres",
        Email = "francaceres28@gmail.com",
    };
    
}