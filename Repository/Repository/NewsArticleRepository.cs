using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public void CreateNewsArticleWithTags(NewsArticle newNewsArticle, List<Tag> taglist)
                 => NewsArticleDAO.CreateNewsArticleWithTags(newNewsArticle, taglist);

        public void DeleteNewsArticle(NewsArticle newNewsArticle)
                => NewsArticleDAO.DeleteNewsArticle(newNewsArticle);

        public NewsArticle GetNewsArticleById(string id)
                => NewsArticleDAO.GetNewsArticleById(id);

        public List<NewsArticle> GetNewsArticles()
                => NewsArticleDAO.GetNewsArticles();

        public List<NewsArticle> GetNewsArticlesByDateRange(DateTime startDate, DateTime endDate)
                => NewsArticleDAO.GetNewsArticlesByDateRange(startDate, endDate);

        public List<NewsArticle> GetNewsArticlesContainTitle(string search)
                => NewsArticleDAO.GetNewsArticlesContainTitle(search);

        public List<NewsArticle> GetNewsArticlesCreatedBy(short accountId)
                => NewsArticleDAO.GetNewsArticlesCreatedBy(accountId);

        public void SaveNewsArticle(NewsArticle newNewsArticle)
                => NewsArticleDAO.SaveNewsArticle(newNewsArticle);


        public void UpdateNewsArticle(NewsArticle newNewsArticle, List<int> tagIds)
                => NewsArticleDAO.UpdateNewsArticle(newNewsArticle, tagIds);

        public void UpdateNewsArticleWithTags(NewsArticle newNewsArticle, List<Tag> taglist)
                => NewsArticleDAO.UpdateNewsArticleWithTags(newNewsArticle, taglist);

    }
}
