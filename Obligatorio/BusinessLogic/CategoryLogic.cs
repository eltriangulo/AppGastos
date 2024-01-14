using Domain;
using Repository;

namespace BusinessLogic;

public class CategoryLogic
{
    private readonly IRepository<Category> _repository;
    
    public CategoryLogic(IRepository<Category> categoryRepository)
    {
        _repository = categoryRepository;
    }
    
    public Category AddCategory(Category category)
    {
        return _repository.Add(category);
    }

    public void DeleteCategory(int name)
    {
        _repository.Delete(name);
    }
    public List<Category> GetAllCategories()
    {
        return _repository.FindAll().ToList();
    }
    
    
    public List<Category> GetAllCategoriesFromUser(string email)
    {
        return _repository.FindAll().Where(x => x.User.Email == email).ToList();
    }
    public List<Category> GetAllCategoriesFromSpace(int idSpace)
    {
        return _repository.FindAll().Where(x => x.Space.Id == idSpace).ToList();
    }

    
    public Category GetCategoryFromSpaceByName(string name, int spaceId)
    {
        var categories = _repository.FindAll(); 
        
        var category = categories.FirstOrDefault(c => c.Name == name && c.Space.Id == spaceId);

        return category;
    }
    
    public void UpdateCategory(Category category)
    {
        _repository.Update(category);
    }
    
    
}