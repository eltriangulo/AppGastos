using BusinessLogic;
using Domain;
using Repository;

namespace BusinessLogicTest;

[TestClass]

public class SessionLogicTest
{
   private SessionLogic _sessionLogic;
   private SqlContext _sqlContext;
   
   [TestInitialize]
   public void SetUp()
   {
       _sessionLogic = new SessionLogic();
   }
   
   [TestMethod]
   public void isLoggedTest()
   {
       User u1 = new User();
         _sessionLogic.SetCurrentUser(u1);
       Assert.AreEqual(true, _sessionLogic.IsLogged());
   }
   
   [TestMethod]
   public void setCurrentUserTest()
   {
       User u1 = new User();
       _sessionLogic.SetCurrentUser(u1);
       Assert.AreEqual(u1, _sessionLogic.GetCurrentUser());
   }

    [TestMethod]
    public void logOutTest()
    {
        User u1 = new User();
        _sessionLogic.SetCurrentUser(u1);
        Assert.AreEqual(true, _sessionLogic.IsLogged());
        _sessionLogic.LogOut();
        Assert.AreEqual(false, _sessionLogic.IsLogged());
    }
    
    [TestMethod]
    public void setCurrentSpaceTest()
    {
        Space s1 = new Space();
        _sessionLogic.SetCurrentSpace(s1);
        Assert.AreEqual(s1, _sessionLogic.GetCurrentSpace());
    }
    
    [TestMethod]
    public void logOutSpaceTest()
    {
        Space s1 = new Space();
        _sessionLogic.SetCurrentSpace(s1);
        Assert.AreEqual(s1, _sessionLogic.GetCurrentSpace());
        _sessionLogic.LogOutSpace();
        Assert.AreEqual(null, _sessionLogic.GetCurrentSpace());
    }
}