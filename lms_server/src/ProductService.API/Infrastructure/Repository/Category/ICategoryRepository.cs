using ProductService.API.Domain;

namespace ProductService.API.Infrastructure.Repository.Category;
public interface ICategoryRepository
{
    Task<Categories?> GetCategoriesById(Guid? id);
    Task<Categories?> IsCategoryExist(string name);
    Task<List<Categories>> GetAllCategories();
    Task<bool> CreateCategory(Categories category);
    Task<bool> UpdateCategory(Categories category);
}