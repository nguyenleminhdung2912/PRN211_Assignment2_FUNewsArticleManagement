using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface INewsArticleRepository
    {
        void SaveNewsArticle(NewsArticle newNewsArticle);

        void DeleteNewsArticle(NewsArticle newNewsArticle);

        void UpdateNewsArticle(NewsArticle newNewsArticle, List<int> tagIds);
        void UpdateNewsArticleWithTags(NewsArticle newNewsArticle, List<Tag> taglist);
        void CreateNewsArticleWithTags(NewsArticle newNewsArticle, List<Tag> taglist);

        List<NewsArticle> GetNewsArticles();

        NewsArticle GetNewsArticleById(string id);

        List<NewsArticle> GetNewsArticlesByDateRange(DateTime startDate, DateTime endDate);

        List<NewsArticle> GetNewsArticlesCreatedBy(short accountId);
        List<NewsArticle> GetNewsArticlesContainTitle(string search);

    }
}
