using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;
namespace BusinessLogicTest;

[TestClass]

public class TransactionLogicTest
{
    private SqlContext _sqlContext;
    
    private TransactionLogic _transactionLogic;
    private IRepository<Transaction> _transactionRepository;
    private User _user;
    private Space _space;
    private Space _space2;
    private Account _account;
    private Account _account2;
    private Category _category;
    private Category _category2;
    private Category _category3;
    private Transaction _transaction;
    private Transaction _transaction2;
    private Transaction _transaction3;
    private Transaction _transaction4;
    private Transaction _transaction5;
    private CreditCard creditCard1;
    
    
    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _transactionRepository = new TransactionMemoryRepository();
        _transactionLogic = new TransactionLogic(_transactionRepository);
        FillAttributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }

    [TestMethod]
    public void AddTransactionTest()
    {
        Transaction returnTransaction = _transactionLogic.AddTransaction(_transaction);
        Assert.AreEqual(_transaction, returnTransaction);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddTransactionWithEmptyTitleTest()
    {
        Transaction tEmpty = new Transaction{
            Id = 1,
            Title = "",
            Date = new DateTime(2023,02,02),
            Amount = 100,
            Currency = "USD",
            Category = _category,
            Account = _account
        };
        _transactionLogic.AddTransaction(tEmpty);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddTransactionWithAmountLessThanZeroTest()
    {
        Transaction tNegative = new Transaction{
            Id = 1,
            Title = "Pedidos Ya order",
            Date = new DateTime(2023,02,02),
            Amount = -100,
            Currency = "USD",
            Category = _category,
            Account = _account
        };
        
        _transactionLogic.AddTransaction(tNegative);
    }

    [TestMethod]
    public void getAllTransactionsFromAccountTest()
    {
        
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        
        Assert.AreEqual(1, _transactionLogic.GetAllTransactionsFromAccount(_account.Name, _space.Id).Count);
    }
   
    [TestMethod]
    public void GetAllTransactionsFromSpaceTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        
        Assert.AreEqual(1, _transactionLogic.GetAllTransactionsFromSpace(_space.Id).Count);
    }
    
    [TestMethod]
    public void getAllTransactionsFromCategoryTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        
        Assert.AreEqual(1, _transactionLogic.GetAllTransactionsFromCategory(_category.Name, _space.Id).Count);
    }
    
    [TestMethod]
    
    public void GetTotalSpentFromCategoryTest()
    {
        Transaction bigTransaction = new Transaction{
            Id = 3,
            Title = "Big transaction",
            Date = new DateTime(2023,10,02),
            Amount = 48000,
            Currency = "UYU",
            Category = _category2,
            Account = _account,
            Space = _space2
        };
        
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(bigTransaction);
        
        Assert.AreEqual(48050, _transactionLogic.GetTotalSpentFromCategoryInMonth(_category2.Name, _space2.Id,new DateTime(2023, 10, 10) ));
    }

    [TestMethod]
    public void GetAllCategoriesWithTransactionsInThatMonthTest()
    {
        
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        
        Assert.AreEqual(2, _transactionLogic.GetCategoriesWithTransactionsInThatMonth(_space2.Id, new DateTime(2023, 10, 10)).Count);
    }
    
    [TestMethod]
    public void GetTotalSpentInMonthTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        
        Assert.AreEqual(100, _transactionLogic.GetTotalSpentInMonth(_space2.Id, new DateTime(2023, 10, 10)));
    }

    [TestMethod]
    public void GetExpensesFromCreditCardTest()
    {
        CreditCard? creditCard = new CreditCard
        {
            IssuingBank = "Itau",
            AvaiableCredit = 2000,
            Last4Digits = "1234",
            CreationDate = DateTime.Now,
            DeadlineDate = DateTime.Now,
        };
        _transaction.CreditCard = creditCard;
        _transaction.Account = null;
        _transaction2.CreditCard = creditCard;
        _transaction2.Account = null;
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);

        Assert.AreEqual(1, _transactionLogic.GetExpensesFromCreditCard(_space2.Id, creditCard.Last4Digits).Count());
    }
    [TestMethod]
    public void GetExpensesIntoDeadlineDateTest()
    {
        CreditCard? creditCard = new CreditCard{IssuingBank = "Itau",AvaiableCredit = 2000,Last4Digits = "1234",
            CreationDate = new DateTime(2021, 02, 02),DeadlineDate = new DateTime(2023,10, 10)
        };
        Transaction t4 = new Transaction{Id = 2,Title = "School Calendar",Date =  new DateTime(2023,08,02),
            Amount = 50,Currency = "UYU",Category = _category,CreditCard = creditCard , Space = _space2};
        
        _transaction2.Account = null;
        _transaction2.CreditCard = creditCard;
        _transaction3.Account = null;
        _transaction3.CreditCard = creditCard;

        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        _transactionLogic.AddTransaction(t4);
        Assert.AreEqual(2, _transactionLogic.GetExpensesIntoDeadlineDate(_space2.Id,creditCard.DeadlineDate, creditCard.Last4Digits).Count());
    }
    [TestMethod]
    public void DeleteTransactionTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        Assert.AreEqual(1, _transactionLogic.GetAllTransactionsFromSpace(_space.Id).Count);
    }

    [TestMethod]
    public void UpdateTransactionTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transaction.Title = "New title";
        _transactionLogic.UpdateTransaction(_transaction);
        Assert.AreEqual("New title", _transactionLogic.GetAllTransactionsFromSpace(_space.Id)[0].Title);
    }
    [TestMethod]
    public void DuplicateTransactionTest()
    {
        Transaction newTransaction = _transactionLogic.DuplicateTransaction(_transaction2);
        Assert.AreEqual(_transaction2.Title, newTransaction.Title);
        Assert.AreEqual(_transaction2.Amount, newTransaction.Amount);
        Assert.AreEqual(_transaction2.Currency, newTransaction.Currency);
        Assert.AreEqual(_transaction2.Category, newTransaction.Category);
        Assert.AreEqual(_transaction2.Account, newTransaction.Account);
        Assert.AreEqual(_transaction2.Space, newTransaction.Space);
    }

    [TestMethod]
    public void GetAllExpensesFromSpaceTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        Assert.AreEqual(1, _transactionLogic.GetAllExpensesFromSpace(_space.Id).Count);
    }
    
    [TestMethod]
    public void GetAllExpensesByCategoryTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        Assert.AreEqual(1, _transactionLogic.GetAllExpensesByCategoryFromSpace(_space.Id, _transaction.Category.Name).Count);
    }

    [TestMethod]
    public void GetAllExpensesByAccountFromSpaceTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        Assert.AreEqual(1, _transactionLogic.GetAllExpensesByAccountFromSpace(_space.Id, _transaction.Account.Name).Count);
    }

    [TestMethod]
    public void GetAllExpensesIntoDateRangeTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        Assert.AreEqual(2, _transactionLogic.GetAllExpensesIntoDateRange(_space2.Id, new DateTime(2023, 09, 10),
            new DateTime(2023, 11, 10)).Count);
    }

    [TestMethod]
    public void GetAllTransactionsFromCreditCardTest()
    {
        _transactionLogic.AddTransaction(_transaction4);
        
        Assert.AreEqual(1, _transactionLogic.GetAllTransactionsFromCreditCard(_transaction4.CreditCard.Last4Digits, _space2.Id).Count);
    }

    [TestMethod]
    public void GetAllExpensesInMonthTest()
    {
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        Assert.AreEqual(2, _transactionLogic.GetAllExpensesInMonth(_space2.Id, new DateTime(2023, 10, 10)).Count);
    }

    [TestMethod]
    public void GetAllIncomesInMonthTest(){
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        _transactionLogic.AddTransaction(_transaction4);
        _transactionLogic.AddTransaction(_transaction5);
        Assert.AreEqual(1, _transactionLogic.GetAllIncomesInMonth(_space2.Id, new DateTime(2023, 10, 10)).Count);
    }
    
    [TestMethod]
    public void GetExpensesPerDayInMonthTest(){
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        _transactionLogic.AddTransaction(_transaction4);
        _transactionLogic.AddTransaction(_transaction5);
        Assert.AreEqual(31, _transactionLogic.GetExpensesPerDayInMonth(_space2.Id, new DateTime(2023, 10, 10)).Count);
    }
    
    [TestMethod]
    public void GetIncomesPerDayInMonthTest(){
        _transactionLogic.AddTransaction(_transaction);
        _transactionLogic.AddTransaction(_transaction2);
        _transactionLogic.AddTransaction(_transaction3);
        _transactionLogic.AddTransaction(_transaction4);
        _transactionLogic.AddTransaction(_transaction5);
        Assert.AreEqual(31, _transactionLogic.GetIncomesPerDayInMonth(_space2.Id, new DateTime(2023, 10, 10)).Count);
    }

   // [TestMethod]
    // public void GetTotalSpentFromCategoriesGoalTest()
    // {
    //     SpendingGoals goal = new SpendingGoals
    //     {
    //         Id = 1,
    //         Title = "t",
    //         MonthlyBudget = 100,
    //         Categories = new List<Category> {_category, _category2},
    //         Space = _space2
    //     };
    //     _transactionLogic.AddTransaction(_transaction);
    //     _transactionLogic.AddTransaction(_transaction2);
    //     _transactionLogic.AddTransaction(_transaction3);
    //     _transactionLogic.AddTransaction(_transaction4);
    //     _transactionLogic.AddTransaction(_transaction5);
    //     Assert.AreEqual(100, _transactionLogic.GetTotalSpentFromCategoriesGoal(goal));
    // }
    public void FillAttributes()
    {
        _user = new User {Name = "Juan", Surname = "Caceres", Email = "j@mail.com"};
        _space = new Space {Name = "mi espacio", Admin = _user, InvitedUsers = {}, Id = 1};
        _space2 = new Space {Name = "mi espacio 2", Admin = _user, InvitedUsers = {}, Id = 2};
        _account = new Account {Name = "Santander", Amount = 250,Currency = "UYU",CreationDate = DateTime.Now,};
        _account2 = new Account {Name = "Brou", Amount = 250,Currency = "UYU",CreationDate = DateTime.Now,};
        _category = new Category{Name = "Food",CreationDate = DateTime.Now,Status = "Active",Type = "Expenses"};
        _category2 = new Category{Name = "School",CreationDate = DateTime.Now,Status = "Active",Type = "Expenses"};
        _category3 = new Category{Name = "Salary",CreationDate = DateTime.Now,Status = "Active",Type = "Income"};
        _transaction = new Transaction{Id = 1,Title = "Pedidos Ya order",Date = DateTime.Now,Amount = 100,
            Currency = "UYU",Category = _category,Account = _account, Space = _space};
        _transaction2 = new Transaction{Id = 2,Title = "Uber",Date =  new DateTime(2023,10,02),Amount = 50,
            Currency = "UYU",Category = _category2,Account = _account2, Space = _space2};
         _transaction3 = new Transaction{Id = 3,Title = "School Calendar",Date =  new DateTime(2023,10,02),Amount = 50,
            Currency = "UYU",Category = _category,Account = _account2, Space = _space2};
         creditCard1 = new CreditCard { IssuingBank = "Itau", AvaiableCredit = 2000, Last4Digits = "1234", 
             CreationDate = DateTime.Now, DeadlineDate = DateTime.Now,
         };
         _transaction4 = new Transaction{Id = 4,Title = "Food",Date =  new DateTime(2023,10,02),Amount = 50,
             Currency = "UYU",Category = _category2, CreditCard = creditCard1, Space = _space2};
         _transaction5 = new Transaction{Id = 5,Title = "Salary",Date =  new DateTime(2023,10,02),Amount = 50,
             Currency = "UYU",Category = _category3,Account = _account2, Space = _space2};
         
        
    }
    
}