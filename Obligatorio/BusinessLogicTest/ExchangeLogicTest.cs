using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;

namespace BusinessLogicTest;

[TestClass]
public class ExchangeLogicTest
{
    private ExchangeLogic _exchangeLogic;
    private IRepository<Exchange?> _exchangeRepository;
    private SqlContext _sqlContext;

    private User _user1;
    private Space _space1;
    private Space _space2;
    private Exchange? _exchange1;
    private Exchange? _exchange2;
    
    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _exchangeRepository = new ExchangeMemoryRepository();
        _exchangeLogic = new ExchangeLogic(_exchangeRepository);
        fillAtributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }

    
    [TestMethod]
    public void AddOneExchangeOkTest()
    {
        Exchange? returnExchange = _exchangeLogic.AddExchange(_exchange1);
        
        Assert.AreEqual(_exchange1, returnExchange);
        Assert.AreEqual(_exchange1.Value, returnExchange.Value);
        Assert.AreEqual(_exchange1.Date, returnExchange.Date);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddExchangeWithNegativeValueUsdTest()
    {
        Exchange? exchangeNegativeUsdValue = new Exchange()
        {
            Value = -40,
            Date = new DateTime(2022, 10, 12)
        };
        
        _exchangeLogic.AddExchange(exchangeNegativeUsdValue);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddExchangeWithZeroValueUsdTest()
    {
        Exchange? exchangeZeroUsdValue = new Exchange()
        {
            Value = 0,
            Date = new DateTime(2022, 10, 12)
        };
        
        _exchangeLogic.AddExchange(exchangeZeroUsdValue);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddExchangeWithNoDateTest()
    {
        Exchange? exchangeNoDate = new Exchange()
        {
            Value = 40,
            Date = new DateTime()
        };
        
        _exchangeLogic.AddExchange(exchangeNoDate);
    }
    [TestMethod]
    public void AddExchangeWithDateInPastTest()
    {
        Exchange? exchangeDateInPast = new Exchange()
        {
            Value = 40,
            Date = new DateTime(2020, 10, 12)
        };
        
        _exchangeLogic.AddExchange(exchangeDateInPast);
    }
    
    [TestMethod]
    public void GetExchangesFromSpaceTest()
    {
        _exchangeLogic.AddExchange(_exchange1);
        _exchangeLogic.AddExchange(_exchange2);
        
        Assert.AreEqual(2, _exchangeLogic.GetExchangesFromSpace(_space1.Id).Count);
        Assert.AreEqual(0, _exchangeLogic.GetExchangesFromSpace(_space2.Id).Count);
    }
    
    [TestMethod]
    public void DeleteExchangeTest()
    {
        _exchangeLogic.AddExchange(_exchange1);
        Assert.AreEqual(1, _exchangeLogic.GetExchangesFromSpace(_space1.Id).Count);
        
        _exchangeLogic.DeleteExchange(_exchange1.Id);
        Assert.AreEqual(0, _exchangeLogic.GetExchangesFromSpace(_space1.Id).Count);
    }
    [TestMethod]
    public void FindExchangeByDateFromSpaceTest()
    {
        _exchangeLogic.AddExchange(_exchange1);
        _exchangeLogic.AddExchange(_exchange2);
        
        Assert.AreEqual(_exchange1, _exchangeLogic.FindExchangeByDateFromSpace(_exchange1.Date, _space1.Id, _exchange1.Currency));
        Assert.AreEqual(null, _exchangeLogic.FindExchangeByDateFromSpace(_exchange1.Date, _space2.Id, _exchange1.Currency));
    }

    private void fillAtributes()
    {
        _user1 = new User {Name = "Juan", Surname = "Caceres", Email = "juan@gmail.com" };
        _space1 = new Space {Name = "mi espacio", Admin = _user1, InvitedUsers = { }, Id = 1 };
        _space2 = new Space {Name = "mi espacio 2", Admin = _user1, InvitedUsers = { }, Id = 2 };
        _exchange1 = new Exchange {Id = 1, Value = 100, Date = new DateTime(2022, 10, 12), Space = _space1};
        _exchange2 = new Exchange {Id = 2,Value = 50, Date = new DateTime(2022, 10, 12), Space = _space1};
    }

}