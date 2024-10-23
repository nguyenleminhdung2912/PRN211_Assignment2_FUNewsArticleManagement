using BusinessObjects;

namespace Repository.IRepository
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category GetCategoryById(short? id);

        void SaveCategory(Category category);

        void DeleteCategory(Category category);

        void UpdateCategory(Category category);

        List<Category> GetCategorysContainName(string search);
    }
}
