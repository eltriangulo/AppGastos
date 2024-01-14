using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;

namespace BusinessLogicTest;


[TestClass]
public class SpaceLogicTest
{
    private SpaceLogic _spaceLogic;
    private IRepository<Space> _spaceRepository;
    private SqlContext _sqlContext;
    
    private User _user1;
    private User _user2;
    private Space _space1;
    private Space _space2;
    

    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _spaceRepository = new SpaceMemoryRepository();
        _spaceLogic = new SpaceLogic(_spaceRepository);
        fillAtributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }


    [TestMethod]
    public void AddOneSpaceOkTest()
    {
        Space returnSpace = _spaceLogic.AddSpace(_space1);

        Assert.AreEqual(_space1, returnSpace);
        Assert.AreEqual(_space1.Name, returnSpace.Name);
        Assert.AreEqual(_space1.Admin, returnSpace.Admin);
        Assert.AreEqual(_space1.InvitedUsers, returnSpace.InvitedUsers);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddSpaceWithEmptyNameTest()
    {
        _space1.Name = "";
        _spaceLogic.AddSpace(_space1);
    }

    [TestMethod]
    public void InviteUserToSpaceTest()
    {
        _spaceLogic.InviteUserToSpace(_space1, _user2);
        _spaceLogic.InviteUserToSpace(_space2, _user1);

        Assert.AreEqual(_space1.InvitedUsers[0], _user2);
        Assert.AreEqual(_space2.InvitedUsers[0], _user1);
    }

    // [TestMethod]
    // public void AssignSpaceIdTest()
    // {
    //     _space1.Id = _spaceLogic.AssignSpaceId();
    //     _spaceLogic.AddSpace(_space1);
    //  
    //     _space2.Id = _spaceLogic.AssignSpaceId();
    //     _spaceLogic.AddSpace(_space2);
    //     Assert.AreEqual(1, _space1.Id);
    //     Assert.AreEqual(2, _space2.Id);
    // }
    
    [TestMethod]
    public void GetAllSpacesFromUserTest()
    {
        _spaceLogic.AddSpace(_space1);
        _spaceLogic.AddSpace(_space2);
        _spaceLogic.InviteUserToSpace(_space2, _user1);
        
        Assert.AreEqual(2, _spaceLogic.GetAllSpacesFromUser(_user1.Id).Count);
        Assert.AreEqual(2, _spaceLogic.GetAllSpacesFromUser(_user2.Id).Count);
    }

    [TestMethod]
    public void ShowInvitedUsersTest()
    {
        _spaceLogic.AddSpace(_space1);
        _spaceLogic.InviteUserToSpace(_space1, _user1);

        string returnString = "Juan Caceres | juan@gmail.com" + "\n";
        Assert.AreEqual(returnString, _spaceLogic.ShowInvitedUsers(_space1));
    }
    
    [TestMethod]
    public void DeleteSpaceTest()
    {
        _spaceLogic.AddSpace(_space1);
        _spaceLogic.AddSpace(_space2);
        Assert.AreEqual(2, _spaceLogic.GetAllSpacesFromUser(_user1.Id).Count);
        _spaceLogic.DeleteSpace(_space1);
        Assert.AreEqual(1, _spaceLogic.GetAllSpacesFromUser(_user1.Id).Count);
    }
    
    [TestMethod]
    
    public void GetDefaultSpaceFromRegisteredUserTest()
    {
        _spaceLogic.AddSpace(_space1);
        _spaceLogic.AddSpace(_space2);
        _spaceLogic.InviteUserToSpace(_space2, _user1);
        Assert.AreEqual(_space1, _spaceLogic.GetDefaultSpaceFromRegisteredUser(_user1.Email));
        Assert.AreEqual(_space2, _spaceLogic.GetDefaultSpaceFromRegisteredUser(_user2.Email));
    }

    private void fillAtributes()
    {
        _user1 = new User {Name = "Juan", Surname = "Caceres", Email = "juan@gmail.com" };
        _user2 = new User {Name = "Franco", Surname = "Caceres", Email = "franco@gmail.com" };
        _space1 = new Space {Name = "mi espacio", Admin = _user1, InvitedUsers = { }, Id = 1 };
        _space2 = new Space {Name = "mi espacio 2", Admin = _user2, InvitedUsers = { }, Id = 2 };
    }
}