using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;

namespace BusinessLogicTest;

[TestClass]

public class SpendingGoalsLogicTest
{
    private SpendingGoalsLogic _spendingGoalsLogic;
    private IRepository<SpendingGoals> _spendingGoalsRepository;
    private SqlContext _sqlContext;

    private User user1;
    private Space space1;
    private SpendingGoals spendingGoals1;
    private SpendingGoals spendingGoals2;
    private Category category1;
    private Category category2;
    
    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _spendingGoalsRepository = new SpendingGoalsMemoryRepository();
        _spendingGoalsLogic = new SpendingGoalsLogic(_spendingGoalsRepository);
        FillAtributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }


    [TestMethod]
    public void AddOneSpendingGoalsOkTest()
    {
        SpendingGoals returnSpendingGoals = _spendingGoalsLogic.AddSpendingGoals(spendingGoals1);

        Assert.AreEqual(spendingGoals1, returnSpendingGoals);
        Assert.AreEqual(spendingGoals1.Title, returnSpendingGoals.Title);
        Assert.AreEqual(spendingGoals1.MonthlyBudget, returnSpendingGoals.MonthlyBudget);
        Assert.AreEqual(spendingGoals1.Categories, returnSpendingGoals.Categories);
        Assert.AreEqual(spendingGoals1.Space, returnSpendingGoals.Space);
        Assert.AreEqual(spendingGoals1.isShared, returnSpendingGoals.isShared);
        Assert.AreEqual(spendingGoals1.Id, returnSpendingGoals.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]

    public void IsSpendingGoalsTitleUniqueTest()
    {
        
        SpendingGoals spendingGoalsRepeatedName = new SpendingGoals()
        {
            Title = "Reduce Food Expenses",
            MonthlyBudget = 100,
            Categories = { category1 },
        };
        
        _spendingGoalsLogic.AddSpendingGoals(spendingGoalsRepeatedName);
        _spendingGoalsLogic.AddSpendingGoals(spendingGoals1);
    }

    [TestMethod]
    public void GetAllSpendingGoalsFromSpaceTest()
    {
        _spendingGoalsLogic.AddSpendingGoals(spendingGoals1);
        _spendingGoalsLogic.AddSpendingGoals(spendingGoals2);
        
        Assert.AreEqual(2, _spendingGoalsLogic.GetAllGoalsFromSpace(spendingGoals1.Space.Id).Count);
    }

    [TestMethod]
    public void DeleteSpendingGoalsTest()
    {
        _spendingGoalsLogic.AddSpendingGoals(spendingGoals1);
        Assert.AreEqual(1, _spendingGoalsLogic.GetAllGoalsFromSpace(spendingGoals1.Space.Id).Count);
        _spendingGoalsLogic.DeleteSpendingGoals(spendingGoals1.Id);
        Assert.AreEqual(0, _spendingGoalsLogic.GetAllGoalsFromSpace(spendingGoals1.Space.Id).Count);
    }

    [TestMethod]
    public void ReturnCategoriesTest()
    {
        SpendingGoals spendingGoals = new SpendingGoals()
        {
            Title = "Reduce Food Expenses",
            MonthlyBudget = 100,
            Categories = { category1, category2 },
        };
        Assert.AreEqual("Food, Car, ", _spendingGoalsLogic.ReturnCategories(spendingGoals));

    }
    
    [TestMethod]
    public void asignIdTest()
    {
        _spendingGoalsLogic.AddSpendingGoals(spendingGoals1);
        Assert.AreEqual(1, _spendingGoalsLogic.AssignedId());
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void DeleteNonExistantSpendingGoalsTest()
    {
        SpendingGoals spendingGoals = new SpendingGoals()
        {
            Title = "Reduce Food Expenses",
            MonthlyBudget = 100,
            Space = space1
        };
        _spendingGoalsLogic.AddSpendingGoals(spendingGoals);
        Assert.AreEqual(1, _spendingGoalsLogic.GetAllGoalsFromSpace(spendingGoals.Space.Id).Count);
        _spendingGoalsLogic.DeleteSpendingGoals(2);
        Assert.AreEqual(1, _spendingGoalsLogic.GetAllGoalsFromSpace(spendingGoals.Space.Id).Count);
    }
    [TestMethod]
    public void IdToSpendingGoalsTest()
    {
      spendingGoals1.Id = 1;
      Assert.AreEqual(1, spendingGoals1.Id);
    }
    [TestMethod]
    public void AddCategoriesToSpendingGoalsTest()
    {
        SpendingGoals goalWithNoCategories = new SpendingGoals()
        {
            Title = "Reduce Food Expenses",
            MonthlyBudget = 100,
            Space = space1
        };
        goalWithNoCategories.Categories.Add(category1);
        goalWithNoCategories.Categories.Add(category2);
        _spendingGoalsLogic.AddSpendingGoals(goalWithNoCategories);
        Assert.AreEqual(2, goalWithNoCategories.Categories.Count);
    }
    
    [TestMethod]
    public void GetSpendingGoalsByIdTest()
    {
        _spendingGoalsLogic.AddSpendingGoals(spendingGoals1);
        Assert.AreEqual(spendingGoals1, _spendingGoalsLogic.GetSpendingGoalsById(spendingGoals1.Id));
    }

    public void FillAtributes()
    {
        user1 = new User {Name = "Juan", Surname = "Caceres", Email = "juan@gmail.com" };
        space1 = new Space {Name = "mi espacio", Admin = user1, InvitedUsers = { }, Id = 1 };
        spendingGoals1 = new SpendingGoals {Title = "Reduce Food Expenses", MonthlyBudget = 100, Space = space1, Categories = {category1, category2}, isShared = false, Id = 1};
        spendingGoals2 = new SpendingGoals {Title = "Reduce Car Expenses", MonthlyBudget = 100, Space = space1, Categories = {category2}, isShared = false, Id = 2};
        category1 = new Category {Name = "Food", CreationDate = DateTime.Now, Status = "Active",
            Type = "Income", User = user1, Space = space1
        };
        category2 = new Category {Name = "Car", CreationDate = DateTime.Now, Status = "Active",
            Type = "Income", User = user1, Space = space1
        };
    }
}