using Domain;

namespace Repository;

public class CategoryMemoryRepository : IRepository<Category>
{
    private List<Category> _categories = new List<Category>();
    public Category Add(Category oneElement)
    {
        _categories.Add(oneElement);
        return oneElement;
    }

    public Category? Find(Func<Category, bool> filter)
    {
        return _categories.Where(filter).FirstOrDefault();
    }

    public IList<Category> FindAll()
    {
        return _categories;
    }

    public Category? Update(Category updatedEntity)
    {
        Category foundCategory = Find(x => x.Name == updatedEntity.Name);
        foundCategory = updatedEntity;
        return foundCategory;
    }
    

    public void Delete(int categoryId)
    {
        _categories.RemoveAll(x => x.Id == categoryId);
    }
    
    
}