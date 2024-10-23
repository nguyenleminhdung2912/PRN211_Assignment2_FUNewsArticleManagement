using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        public static void DeleteCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public static List<Category> GetCategories()
        {
            var listCategory = new List<Category>();
            try
            {
                using var db = new FunewsManagementFall2024Context();
                listCategory = db.Categories
                    .Include(n => n.NewsArticles)
                    .ToList();
            }
            catch (Exception ex) { }
            return listCategory;
        }

        public static Category GetCategoryById(short? id)
        {
            using var db = new FunewsManagementFall2024Context();
            Category category = new Category();
            category = db.Categories
                .Include(n => n.NewsArticles)
                .FirstOrDefault(c => c.CategoryId == id);
            return category;
        }

        public static List<Category> GetCategorysContainName(string search)
        {
            using (var context = new FunewsManagementFall2024Context())
            {
                return context.Categories
                    .Where(c => c.CategoryName.Contains(search))
                    .Include(n => n.NewsArticles)
                    .ToList();
            }
        }

        public static void SaveCategory(Category category)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                context.Categories.Add(category);
                context.SaveChanges();
                category.ParentCategoryId = category.CategoryId;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateCategory(Category category)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                context.Entry<Category>(category).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
