using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;

namespace Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public void DeleteCategory(Category category)
                => CategoryDAO.DeleteCategory(category);


        public List<Category> GetCategories()
                => CategoryDAO.GetCategories();

        public Category GetCategoryById(short? id)
                => CategoryDAO.GetCategoryById(id);

        public List<Category> GetCategorysContainName(string search)
                => CategoryDAO.GetCategorysContainName(search);


        public void SaveCategory(Category category)
                => CategoryDAO.SaveCategory(category);


        public void UpdateCategory(Category category)
                => CategoryDAO.UpdateCategory(category);

    }
}
