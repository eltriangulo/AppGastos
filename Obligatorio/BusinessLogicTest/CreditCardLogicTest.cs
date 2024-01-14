using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;

namespace BusinessLogicTest;

[TestClass]
public class CreditCardLogicTest
{
    private CreditCardLogic _creditCardLogic;
    private IRepository<CreditCard?> _creditCardRepository;
    private SqlContext _sqlContext;
    
    private User user1;
    private Space space1;
    private Space space2;
    private CreditCard? creditCard1;
    private CreditCard? creditCard2;
    
    [TestInitialize]
    public void SetUp()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _sqlContext = sqlContextFactory.CreateMemoryContext();
        
        _creditCardRepository = new CreditCardMemoryRepository();
        _creditCardLogic = new CreditCardLogic(_creditCardRepository);
        FillAtributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext.Database.EnsureDeleted();
    }


    [TestMethod]
    public void AddOneCreditCardOkTest()
    {
        CreditCard? returnCreditCard = _creditCardLogic.AddCreditCard(creditCard1);
        
        Assert.AreEqual(creditCard1, returnCreditCard);
        Assert.AreEqual(creditCard1.IssuingBank, returnCreditCard.IssuingBank );
        Assert.AreEqual(creditCard1.Last4Digits, returnCreditCard.Last4Digits);
        Assert.AreEqual(creditCard1.Currency, returnCreditCard.Currency);
        Assert.AreEqual(creditCard1.AvaiableCredit, returnCreditCard.AvaiableCredit );
        Assert.AreEqual(creditCard1.CreationDate, returnCreditCard.CreationDate);
        Assert.AreEqual(creditCard1.DeadlineDate, returnCreditCard.DeadlineDate);
        Assert.AreEqual(creditCard1.User, returnCreditCard.User);

    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddCreditCardWithEmptyIssuingBankTest()
    {
        CreditCard? creditCardEmptyBank = new CreditCard()
        {
            IssuingBank = "",
            Last4Digits = "1234",
            Currency = "UYU",
            AvaiableCredit = 3800,
            CreationDate = new DateTime(2021, 02, 02),
            DeadlineDate = new DateTime(2021, 03, 03),
        };
        _creditCardLogic.AddCreditCard(creditCardEmptyBank);
    }

    [TestMethod]
    public void GetAllCreditCardsFromSpaceTest()
    {
        _creditCardLogic.AddCreditCard(creditCard1);
        _creditCardLogic.AddCreditCard(creditCard2);
        
        Assert.AreEqual(2, _creditCardLogic.GetAllCreditCardsFromSpace(space1.Id).Count);
        Assert.AreEqual(0, _creditCardLogic.GetAllCreditCardsFromSpace(space2.Id).Count);
    }

    [TestMethod]
    public void DeleteCreditCardTest()
    {
        _creditCardLogic.AddCreditCard(creditCard1);
        Assert.AreEqual(1, _creditCardLogic.GetAllCreditCardsFromSpace(space1.Id).Count);
        
        _creditCardLogic.DeleteCreditCard(Int32.Parse(creditCard1.Last4Digits));
        Assert.AreEqual(1, _creditCardLogic.GetAllCreditCardsFromSpace(space1.Id).Count);
    }
    
    [TestMethod]
    public void GetCreditCardTest()
    {
        _creditCardLogic.AddCreditCard(creditCard1);
        _creditCardLogic.AddCreditCard(creditCard2);
        
        Assert.AreEqual(creditCard1, _creditCardLogic.GetCreditCard(space1.Id, creditCard1.Last4Digits));
        Assert.AreEqual(creditCard2, _creditCardLogic.GetCreditCard(space1.Id, creditCard2.Last4Digits));
    }
    
    [TestMethod]
    public void UpdateCreditCardTest()
    {
        _creditCardLogic.AddCreditCard(creditCard1);
        Assert.AreEqual(creditCard1, _creditCardLogic.GetCreditCard(space1.Id, creditCard1.Last4Digits));
        
        creditCard1.AvaiableCredit = 5000;
        _creditCardLogic.UpdateCreditCard(creditCard1);
        Assert.AreEqual(creditCard1, _creditCardLogic.GetCreditCard(space1.Id, creditCard1.Last4Digits));
    }

    [TestMethod]
    public void UpdateCreditCardAmountTest()
    {
        _creditCardLogic.AddCreditCard(creditCard1);
        Assert.AreEqual(creditCard1, _creditCardLogic.GetCreditCard(space1.Id, creditCard1.Last4Digits));
        
        Category category1 = new Category()
        {
            Name = "Comida",
            Type = "Expenses",
            CreationDate = new DateTime(2021, 02, 02),
            Status = "Active",
            Space = space1,
            User = user1
        };
        
        _creditCardLogic.UpdateCreditCardAmount(creditCard1, 1000, category1);
        Assert.AreEqual(creditCard1, _creditCardLogic.GetCreditCard(space1.Id, creditCard1.Last4Digits));
        Assert.AreEqual(creditCard1.AvaiableCredit, 3000);
        
        Category category2 = new Category()
        {
            Name = "Sueldo",
            Type = "Income",
            CreationDate = new DateTime(2021, 02, 02),
            Status = "Active",
            Space = space1,
            User = user1
        };
        
        _creditCardLogic.UpdateCreditCardAmount(creditCard1, 1000, category2);
        Assert.AreEqual(creditCard1, _creditCardLogic.GetCreditCard(space1.Id, creditCard1.Last4Digits));
        Assert.AreEqual(creditCard1.AvaiableCredit, 4000);
    }
    

    public void FillAtributes()
    {
        user1 = new User {Name = "Juan", Surname = "Caceres", Email = "juan@gmail.com"};
        space1 = new Space {Name = "mi espacio", Admin = user1, InvitedUsers = {}, Id = 1};
        space2 = new Space {Name = "mi espacio 2", Admin = user1, InvitedUsers = {}, Id = 2};
        creditCard1 = new CreditCard { IssuingBank = "ITAU", Last4Digits = "1274", Currency = "UYU", AvaiableCredit = 4000, Space = space1};
        creditCard2 = new CreditCard { IssuingBank = "BROU", Last4Digits = "3344", Currency = "UYU", AvaiableCredit = 5000, Space = space1};
    }
}