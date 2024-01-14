using System.ComponentModel.Design.Serialization;
using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;


namespace BusinessLogicTest;

[TestClass]

public class AccountLogicTest
{
    private AccountLogic _accountLogic;
    private AccountMemoryRepository _accountRepository;
    private SqlContext _sqlContext;


    private User user;
    private Space space1;
    private Space space2;
    private Account? account1;
    private Account? account2;
    private Transaction transaction1;
    private Transaction transaction2;
    private Category category1;
    private Category category2;

    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _accountRepository = new AccountMemoryRepository();
        _accountLogic = new AccountLogic(_accountRepository);
        FillAtributes();
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AddOneAccountOkTest()
    {
        Account? returnAccount = _accountLogic.AddAccount(account1);
        
        Assert.AreEqual(account1, returnAccount);
        Assert.AreEqual(account1.Name, returnAccount.Name);
        Assert.AreEqual(account1.CreationDate, returnAccount.CreationDate);
        Assert.AreEqual(account1.Amount, returnAccount.Amount);
        Assert.AreEqual(account1.Currency, returnAccount.Currency );
        Assert.AreEqual(account1.User, returnAccount.User);
        Assert.AreEqual(account1.Space, returnAccount.Space);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddAccountWithEmptyNameTest()
    {
        Account? returnAccount = new Account {Name = "", Amount = 250,Currency = "UYU",
            CreationDate =  new DateTime(2023,02,02), Space = space1};
        _accountLogic.AddAccount(returnAccount);
    }
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddAcountWithNegativeAmountTest()
    {
        Account? returnAccount = new Account {Name = "Santander", Amount = -250,Currency = "UYU",
            CreationDate =  new DateTime(2023,02,02), Space = space1};
        _accountLogic.AddAccount(returnAccount);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddAccountWithEmptyCurrencyTest()
    {
        Account? returnAccount = new Account {Name = "Santander", Amount = 250,Currency = "",
            CreationDate =  new DateTime(2023,02,02), Space = space1};
        _accountLogic.AddAccount(returnAccount);
    }
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddAccountWithFutureCreationDateTest()
    {
        Account? returnAccount = new Account {Name = "Santander", Amount = 250,Currency = "UYU",
            CreationDate =  new DateTime(2030,02,02), Space = space1};
        _accountLogic.AddAccount(returnAccount);
    }
    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void isRepeatedNameTest()
    {
        Account? returnAccount = new Account {Name = "Santander", Amount = 250,Currency = "UYU",
            CreationDate =  new DateTime(2023,02,02), Space = space1};
        _accountLogic.AddAccount(account1);
        _accountLogic.AddAccount(returnAccount);
    }
    

    [TestMethod]
    public void getAllAccountsFromSpaceTest()
    {
        Account? accountDiffSpace = new Account {Name = "Itau", Amount = 250,Currency = "UYU",
            CreationDate =  new DateTime(2023,02,02), Space = space2};
        
        _accountLogic.AddAccount(accountDiffSpace);
        _accountLogic.AddAccount(account1);
        _accountLogic.AddAccount(account2);
        
        Assert.AreEqual(2, _accountLogic.GetAllAccountsFromSpace(space1.Id).Count);
        
    }

    [TestMethod]
    public void deleteAccountTest()
    {
        _accountLogic.AddAccount(account1);
        Assert.AreEqual(1, _accountLogic.GetAllAccountsFromSpace(space1.Id).Count());
        _accountLogic.DeleteAccount(account1.Id);
        Assert.AreEqual(0, _accountLogic.GetAllAccountsFromSpace(space1.Id).Count());

    }

    [TestMethod]
    public void UpdateAccountAmountTest()
    {
        _accountLogic.AddAccount(account1);
        _accountLogic.UpdateAccountAmount(account1, transaction1.Amount, transaction1.Category);
        Assert.AreEqual(200, account1.Amount);

        _accountLogic.AddAccount(account2);
        _accountLogic.UpdateAccountAmount(account2, transaction2.Amount, transaction2.Category);
        Assert.AreEqual(500, account2.Amount);
    }
    
    [TestMethod]
    
    public void GetAccountFromSpaceByNameTest()
    {
        _accountLogic.AddAccount(account1);
        _accountLogic.AddAccount(account2);
        Account? account = _accountLogic.GetAccountFromSpaceByName(account1.Name, space1.Id);
        Assert.AreEqual(account1, account);
    }
    
    [TestMethod]
    
    public void updateAccountTest()
    {
        _accountLogic.AddAccount(account1);
        _accountLogic.AddAccount(account2);
        Account? account = _accountLogic.GetAccountFromSpaceByName(account1.Name, space1.Id);
        account.Name = "Nuevo nombre";
        _accountLogic.UpdateAccount(account);
        Assert.AreEqual("Nuevo nombre", account.Name);
    }

    public void FillAtributes()
    {
        user = new User {Name = "Juan", Surname = "Caceres", Email = "juan@gmail.com"};
        space1 = new Space {Name = "mi espacio", Admin = user, InvitedUsers = {}, Id = 1};
        space2 = new Space {Name = "mi espacio 2", Admin = user, InvitedUsers = {}, Id = 2};
        account1 = new Account {Name = "Santander", Amount = 500,Currency = "UYU",CreationDate =  new DateTime(2023,02,02), Space = space1, User = user};
        account2 = new Account {Name = "Brou", Amount = 300,Currency = "USD",CreationDate =  new DateTime(2023,02,02), Space = space1, User = user};
        category1 = new Category {Name = "Food", Type = "Expenses", Space = space1};
        category2 = new Category {Name = "Food", Type = "Income", Space = space1};
        transaction1 = new Transaction {Id = 1, Title = "Pedidos Ya order", Date = new DateTime(2023,02,02), Amount = 300, 
            Currency = "UYU", Account = account1, Category = category1};
        transaction2 = new Transaction {Id = 1, Title = "Vodka", Date = new DateTime(2023,02,02), Amount = 200, 
            Currency = "USD", Account = account1, Category = category2};
        
    }
}