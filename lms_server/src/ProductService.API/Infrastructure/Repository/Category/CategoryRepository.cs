using Microsoft.EntityFrameworkCore;
using ProductService.API.Domain;
using ProductService.API.Infrastructure.Database;

namespace ProductService.API.Infrastructure.Repository.Category;
public class CategoryRepository : ICategoryRepository
{
    private readonly CampingDbContext _dbContext;
    public CategoryRepository(CampingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateCategory(Categories category)
    {
        await _dbContext.Categories.AddAsync(category);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<Categories>> GetAllCategories()
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Categories?> GetCategoriesById(Guid? id)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Categories?> IsCategoryExist(string name)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public async Task<bool> UpdateCategory(Categories category)
    {
        _dbContext.Categories.Update(category);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}