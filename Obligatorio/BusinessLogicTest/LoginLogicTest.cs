using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;

namespace BusinessLogicTest;

[TestClass]
public class LoginLogicTest
{
    private UserLogic _userLogic;
    private IRepository<User> _userRepository;
    private LogInLogic _loginLogic;
    private SqlContext _sqlContext;

    private User user1;

    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _userRepository = new UserMemoryRepository();
        _userLogic = new UserLogic(_userRepository);
        _loginLogic = new LogInLogic(_userRepository);
        FillAtributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }


    [TestMethod]
    public void LoginTest()
    {
        user1.SetPassword("1234567890A");
        _userLogic.AddUser(user1);
        Assert.AreEqual(user1, _loginLogic.SearchUser(user1.Email, user1.GetPassword()));
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void SearchUserTest()
    {
        user1.SetPassword("1234567890A");
        _userLogic.AddUser(user1);
        Assert.AreEqual(user1, _loginLogic.SearchUser(user1.Email, "incorrtectPassword"));
    }

    public void FillAtributes()
    {
        user1 = new User {Name = "Juan", Surname = "Caceres", Email = "juan@gmail.com"};
    }
}
    
    
        