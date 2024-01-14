using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;

namespace BusinessLogicTest;

[TestClass]
public class UserLogicTest
{
    private SqlContext _sqlContext;
    private UserLogic _userLogic;
    private IRepository<User> _userRepository;
    private User u1;
    private User u2;

    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _userRepository = new UserMemoryRepository();
        _userLogic = new UserLogic(_userRepository);
        FillAttributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }


    [TestMethod]
    public void AddUserTest()
    {
        User returnUser = _userLogic.AddUser(u1);
        Assert.AreEqual(u1, returnUser);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddUserWithEmptyNameTest()
    {
        u1.Name = "";
        _userLogic.AddUser(u1);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddUserWithEmptySurnameTest()
    {
        u1.Surname = "";
        _userLogic.AddUser(u1);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddUserWithEmptyEmailTest()
    {
        u1.Email = "";
        _userLogic.AddUser(u1);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddUserWithEmptyPasswordTest()
    {
        u1.SetPassword("");
        _userLogic.AddUser(u1);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void AddUserWithInvalidEmailTest()
    {
        u1.Email = "francaceres28gmail.com";
        _userLogic.AddUser(u1);
    }

    [TestMethod]
    public void AddUserWithValidEmailTest()
    {
        Assert.AreEqual(u1, _userLogic.AddUser(u1));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void IsRepeatedEmailTest()
    {
        u2.Email = "francaceres28@gmail.com";
        _userLogic.AddUser(u1);
        _userLogic.AddUser(u2);
    }

    [TestMethod]
    public void PasswordFormatOkTest()
    {
        u1.SetPassword("M12345678901");
        Assert.AreEqual(true, _userLogic.PasswordFormatOk("M12345678901"));
    }
    
    [TestMethod]

    public void GetUserByEmailTest()
    {
        _userLogic.AddUser(u1);
        Assert.AreEqual(u1, _userLogic.GetUserByEmail("francaceres28@gmail.com"));
        
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void GetUserByEmailTestIfNull()
    {
        _userLogic.AddUser(u1); 
        Assert.AreEqual(u1, _userLogic.GetUserByEmail("franca28@gmail.com"));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void PasswordAreTheSameTest()
    {
        _userLogic.PasswordAreTheSame("password1", "password2");
        _userLogic.PasswordAreTheSame(u1.GetPassword(), u2.GetPassword());
    }

    [TestMethod]
    public void UpdateUserTest()
    {
        _userLogic.AddUser(u1);
        u1.Name = "Franco";
        u1.Surname = "Caceres";
        u1.Email = "f@gmai.com";
        _userLogic.UpdateUser(u1);
    }

    public void FillAttributes()
        {
            u1 = new User { Name = "Franco", Surname = "Caceres", Email = "francaceres28@gmail.com" };
            u2 = new User { Name = "Tiago", Surname = "Abenante", Email = "fran28@gmail.com" };
        }

    
}	