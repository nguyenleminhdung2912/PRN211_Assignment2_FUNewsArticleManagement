using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class TagDAO
    {
        public static void Delete(Tag tag)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                var news1 =
                    context.Tags.SingleOrDefault(c => c.TagId == tag.TagId);
                context.Tags.Remove(news1);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Tag GetTagById(int id)
        {
            using var context = new FunewsManagementFall2024Context();
            return context.Tags
                .Include(n => n.NewsArticles)
                .FirstOrDefault(c => c.TagId.Equals(id));
        }

        public static List<Tag> GetTags()
        {
            var list = new List<Tag>();
            try
            {
                using var context = new FunewsManagementFall2024Context();
                list = context.Tags
                    .Include(n => n.NewsArticles)
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }

        public static void SaveTag(Tag tag)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                context.Tags.Add(tag);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateTag(Tag tag)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                context.Entry<Tag>(tag).State
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
