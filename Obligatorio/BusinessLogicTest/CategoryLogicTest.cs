using BusinessLogic;
using BusinessLogicTest.Context;
using Domain;
using Repository;

namespace BusinessLogicTest;

[TestClass]
public class CategoryLogicTest
{
	private CategoryLogic _categoryLogic;
    private IRepository<Category> _categoryRepository;
    private SqlContext _sqlContext;

    private User _user1;
    private Space _space1;
    private Category _category1;
    private Category _category2;
    
    
    [TestInitialize]
    public void SetUp()
    {
	    SqlContextFactory sqlContextFactory = new SqlContextFactory();
	    _sqlContext = sqlContextFactory.CreateMemoryContext();
	    
	    _categoryRepository = new CategoryMemoryRepository();
	    _categoryLogic = new CategoryLogic(_categoryRepository);
	    fillAtributes();
    }
    [TestCleanup]
    public void CleanUp()
    {
	    _sqlContext.Database.EnsureDeleted();
    }


    [TestMethod]

    public void AddOneCategoryOkTest()
    {
	    Category returnCategory = _categoryLogic.AddCategory(_category1);

	    Assert.AreEqual(_category1.Name, returnCategory.Name);	
	    Assert.AreEqual(_category1.CreationDate, returnCategory.CreationDate);
	    Assert.AreEqual(_category1.Status, returnCategory.Status);
	    Assert.AreEqual(_category1.Type, returnCategory.Type);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void AddCategoryWithEmptyNameTest()
	{
	    Category categoryEmptyName = new Category()
	    {
		    Name = "",
		    CreationDate = DateTime.Now,
		    Status = "Active",
		    Type = "Income",
	    };
	    _categoryLogic.AddCategory(categoryEmptyName);
	}
    
    [TestMethod]
    public void ListCategoriesTest()
	{
	    _categoryLogic.AddCategory(_category1);
		_categoryLogic.AddCategory(_category2);
	    
	    List<Category> categoriesList = _categoryLogic.GetAllCategories();
	    
	    Assert.AreEqual(2, categoriesList.Count);
	}

    [TestMethod]
    public void ListCategoriesFromUserTest()
    {
	    _categoryLogic.AddCategory(_category1);
	    _categoryLogic.AddCategory(_category2);
	    Assert.AreEqual(2, _categoryLogic.GetAllCategoriesFromUser(_user1.Email).Count);
    }
    
    [TestMethod]
    public void IsActiveCategoryTest()
    {
	    _categoryLogic.AddCategory(_category1);
	    Assert.AreEqual(true, _category1.IsActiveCategory());
    }

    [TestMethod]
    public void GetAllCategoriesFromSpaceTest()
    {
	    _categoryLogic.AddCategory(_category1);
	    _categoryLogic.AddCategory(_category2);
	    
	    Assert.AreEqual(2, _categoryLogic.GetAllCategoriesFromSpace(_space1.Id).Count);
	    
    }
    
    [TestMethod]
    public void DeleteCategoryTest()
	{
	    _categoryLogic.AddCategory(_category1);
	    Assert.AreEqual(1, _categoryLogic.GetAllCategories().Count);
	    _categoryLogic.DeleteCategory(_category1.Id);
	    Assert.AreEqual(0, _categoryLogic.GetAllCategories().Count);
	}
    
    [TestMethod]
    public void GetCategoryFromSpaceByNameTest()
    {
	    _categoryLogic.AddCategory(_category1);
	    Assert.AreEqual(_category1.Name, _categoryLogic.GetCategoryFromSpaceByName(_category1.Name, 
		    _space1.Id).Name);
    }
    
    [TestMethod]
    public void UpdateCategoryTest()
	{
	    _categoryLogic.AddCategory(_category1);
	    _category1.Name = "Avatar";
	    _categoryLogic.UpdateCategory(_category1);
	    Assert.AreEqual("Avatar", _categoryLogic.GetCategoryFromSpaceByName("Avatar", _space1.Id).Name);
	}


    private void fillAtributes()
    {
	    _user1 = new User {Name = "Juan", Surname = "Caceres", Email = "juan@gmail.com" };
	    _space1 = new Space {Name = "mi espacio", Admin = _user1, InvitedUsers = { }, Id = 1 };
	    _category1 = new Category {Name = "Avatar", CreationDate = DateTime.Now, Status = "Active",
		    Type = "Income", User = _user1, Space = _space1
	    };
	    _category2 = new Category {Name = "Food", CreationDate = DateTime.Now, Status = "Active",
		    Type = "Income", User = _user1, Space = _space1
	    };
    }
}

