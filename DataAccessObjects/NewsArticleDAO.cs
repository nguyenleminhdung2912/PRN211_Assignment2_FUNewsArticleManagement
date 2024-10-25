using Azure;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;


namespace DataAccessObjects
{
    public class NewsArticleDAO
    {
        TagDAO tagDAO = new TagDAO();

        public static List<NewsArticle> GetNewsArticles()
        {
            var list = new List<NewsArticle>();
            try
            {
                using var context = new FunewsManagementFall2024Context();
                list = context.NewsArticles
                    .Include(n => n.Tags)
                    .Include(n => n.Category)
                    .Include(n => n.CreatedBy)
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }

        public static void UpdateNewsDelete(NewsArticle newsArticle)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                context.Entry<NewsArticle>(newsArticle).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static NewsArticle GetNewsArticleById(string id)
        {
            using var context = new FunewsManagementFall2024Context();
            return context.NewsArticles
                .Include(n => n.Tags)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefault(c => c.NewsArticleId.Equals(id));
        }

        public static void SaveNewsArticle(NewsArticle newNewsArticle)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();

                // Kiểm tra nếu NewsArticleId đã tồn tại
                var existingArticle = context.NewsArticles
                    .SingleOrDefault(a => a.NewsArticleId == newNewsArticle.NewsArticleId);

                if (existingArticle != null)
                {
                    // Nếu tồn tại, có thể xóa hoặc cập nhật theo ý bạn
                    // context.NewsArticles.Remove(existingArticle); // Nếu muốn xóa
                    // context.SaveChanges(); // Nếu đã xóa
                    // Hoặc cập nhật
                    // existingArticle.Title = newNewsArticle.Title; // Cập nhật các thuộc tính khác
                    // context.SaveChanges(); // Lưu thay đổi
                }
                else
                {

                    // Generate unique NewsArticleId
                    var maxId = context.NewsArticles.Max(u => u.NewsArticleId) ?? "0"; // Handle null case
                    int newId = int.Parse(maxId) + 1; // Generate new ID
                    newNewsArticle.NewsArticleId = newId.ToString(); // Ensure it's stored as a numeric string
                                                                     // Nếu không tồn tại, thêm mới
                    context.Entry(newNewsArticle).State = EntityState.Added;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteNewsArticle(NewsArticle newNewsArticle)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                var news1 =
                    context.NewsArticles.SingleOrDefault(c => c.NewsArticleId == newNewsArticle.NewsArticleId);
                context.NewsArticles.Remove(news1);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateNewsArticle(NewsArticle updateNewsArticle, List<int> tagIds)
        {
            using var _context = new FunewsManagementFall2024Context();

            // Lấy bài viết hiện tại từ database, bao gồm cả Tags để cập nhật
            NewsArticle newsArticleFromDB = _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefault(n => n.NewsArticleId.Equals(updateNewsArticle.NewsArticleId));

            // Cập nhật thông tin bài viết
            newsArticleFromDB.NewsTitle = updateNewsArticle.NewsTitle;
            newsArticleFromDB.Headline = updateNewsArticle.Headline;
            newsArticleFromDB.NewsContent = updateNewsArticle.NewsContent;
            newsArticleFromDB.NewsSource = updateNewsArticle.NewsSource;
            newsArticleFromDB.Category = updateNewsArticle.Category;
            newsArticleFromDB.CategoryId = updateNewsArticle.CategoryId;
            newsArticleFromDB.ModifiedDate = updateNewsArticle.ModifiedDate;
            newsArticleFromDB.UpdatedById = updateNewsArticle.UpdatedById;

            // Cập nhật Tags
            newsArticleFromDB.Tags.Clear(); // Xóa tất cả các Tags hiện tại

            var newTags = _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
            foreach (var tag in newTags)
            {
                newsArticleFromDB.Tags.Add(tag); // Thêm từng Tag mới
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.SaveChangesAsync();
        }


        public static List<NewsArticle> GetNewsArticlesByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var context = new FunewsManagementFall2024Context())
            {
                return context.NewsArticles
                    .Include(article => article.Tags)
                    .Where(article => article.CreatedDate >= startDate && article.CreatedDate <= endDate)
                    .OrderByDescending(article => article.CreatedDate)
                    .ToList();
            }
        }

        public static List<NewsArticle> GetNewsArticlesCreatedBy(short accountId)
        {
            using (var context = new FunewsManagementFall2024Context())
            {
                return context.NewsArticles
                    .Include(article => article.Tags)
                    .Include(article => article.Category)
                    .Where(article => article.CreatedById == accountId)
                    .ToList();
            }
        }

        public static List<NewsArticle> GetNewsArticlesContainTitle(string search)
        {
            using (var context = new FunewsManagementFall2024Context())
            {
                return context.NewsArticles
                    .Where(article => article.NewsTitle.Contains(search))
                    .ToList();
            }
        }

        public static void UpdateNewsArticleWithTags(NewsArticle updateNewsArticle, List<Tag> taglist)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();

                // Article from db
                var newsArticleFromDB
                    = context.NewsArticles
                    .Include(article => article.Tags) // Include existing tags
                    .FirstOrDefault(article => article.NewsArticleId.Equals(updateNewsArticle.NewsArticleId));

                if (newsArticleFromDB == null)
                {
                    throw new Exception("NewsArticle not found");
                }

                // Transfer data from updateNewsArticle to newsArticleFromDB
                newsArticleFromDB.NewsTitle = updateNewsArticle.NewsTitle;
                newsArticleFromDB.Headline = updateNewsArticle.Headline;
                newsArticleFromDB.NewsContent = updateNewsArticle.NewsContent;
                newsArticleFromDB.NewsSource = updateNewsArticle.NewsSource;
                newsArticleFromDB.Category = updateNewsArticle.Category;
                newsArticleFromDB.CategoryId = updateNewsArticle.CategoryId;
                newsArticleFromDB.ModifiedDate = updateNewsArticle.ModifiedDate;
                newsArticleFromDB.UpdatedById = updateNewsArticle.UpdatedById;
                newsArticleFromDB.Tags = taglist;

                context.Attach(newsArticleFromDB).State = EntityState.Modified;

                // Remove tags that are not in the new tag list
                var tagsToRemove = newsArticleFromDB.Tags
                    .Where(t => !taglist.Any(tag => tag.TagId == t.TagId))
                    .ToList();

                // Add new tags that are not in the current list
                foreach (var tag in taglist)
                {
                    // Kiểm tra xem tag đã tồn tại trong danh sách Local của context hay chưa
                    var existingTag = context.Tags.Local.FirstOrDefault(t => t.TagId == tag.TagId);

                    if (existingTag == null)
                    {
                        // Tìm tag trong database mà không theo dõi (AsNoTracking)
                        existingTag = context.Tags.AsNoTracking().FirstOrDefault(t => t.TagId == tag.TagId);

                        if (existingTag != null)
                        {
                            // Gắn kết thực thể đã tìm thấy vào context
                            context.Attach(existingTag);
                        }
                    }

                    if (existingTag != null)
                    {
                        // Nếu tag tồn tại, thêm nó vào newsArticleFromDB.Tags nếu chưa có
                        if (!newsArticleFromDB.Tags.Any(t => t.TagId == existingTag.TagId))
                        {
                            newsArticleFromDB.Tags.Add(existingTag);
                        }
                    }
                    else
                    {
                        // Nếu tag không tồn tại trong database, thêm mới tag vào newsArticleFromDB.Tags
                        newsArticleFromDB.Tags.Add(tag);
                    }
                }


                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewsArticleWithTags(NewsArticle newNewsArticle, List<Tag> taglist)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();

                // Set news status
                newNewsArticle.NewsStatus = true;

                // Generate unique NewsArticleId
                var maxId = context.NewsArticles.Max(u => u.NewsArticleId) ?? "0"; // Handle null case
                int newId = int.Parse(maxId) + 1; // Generate new ID
                newNewsArticle.NewsArticleId = newId.ToString(); // Ensure it's stored as a numeric string

                // Handle tags
                foreach (var tag in taglist)
                {
                    var existingTag = context.Tags.Local.FirstOrDefault(t => t.TagId == tag.TagId)
                                      ?? context.Tags.Find(tag.TagId);
                    if (existingTag != null)
                    {
                        newNewsArticle.Tags.Add(existingTag); // Associate existing tag
                    }
                    else
                    {
                        context.Tags.Add(tag); // Add new tag
                        newNewsArticle.Tags.Add(tag);
                    }
                }

                // Add the new article to the context
                context.NewsArticles.Add(newNewsArticle);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "No inner exception.";
                throw new Exception($"Error: {ex.Message}, Inner Exception: {innerException}");
            }
        }

        public static void AddTag(NewsArticle currentArticle, List<int> SelectedTagIds)
        {
            try
            {
                using var funewsManagementFall2024Context = new FunewsManagementFall2024Context();

                // Load current tags and their associations
                var existingTags = funewsManagementFall2024Context.Tags
                    .Include(n => n.NewsArticles)
                    .Where(t => SelectedTagIds.Contains(t.TagId)).ToList();

                // Clear current article's tags before adding new ones
                currentArticle.Tags.Clear();

                foreach (var tag in existingTags)
                {
                    // Add the tag to the article's tags collection if it's not already there
                    if (!currentArticle.Tags.Contains(tag))
                    {
                        currentArticle.Tags.Add(tag);
                        tag.NewsArticles.Add(currentArticle);
                    }
                }

                // Mark the article as modified and save changes
                funewsManagementFall2024Context.Entry(currentArticle).State = EntityState.Modified;
                funewsManagementFall2024Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void RemoveTag(NewsArticle currentArticle, List<int> SelectedTagIds)
        {
            try
            {
                using var funewsManagementFall2024Context = new FunewsManagementFall2024Context();

                // Lấy bài báo hiện tại từ cơ sở dữ liệu thay vì từ tham số
                var trackedArticle = funewsManagementFall2024Context.NewsArticles
                    .Include(n => n.Tags)
                    .FirstOrDefault(a => a.NewsArticleId == currentArticle.NewsArticleId);

                // Nếu không tìm thấy bài báo, trả về lỗi hoặc xử lý
                if (trackedArticle == null)
                {
                    throw new Exception("Bài báo không tồn tại.");
                }

                // Lấy các thẻ hiện có
                var existingTags = funewsManagementFall2024Context.Tags
                    .Include(n => n.NewsArticles)
                    .Where(t => SelectedTagIds.Contains(t.TagId)).ToList();

                // Xóa các thẻ khỏi bài báo
                foreach (var tag in existingTags)
                {
                    if (trackedArticle.Tags.Contains(tag))
                    {
                        trackedArticle.Tags.Remove(tag);
                        tag.NewsArticles.Remove(trackedArticle);
                    }
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                funewsManagementFall2024Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa thẻ: {ex.Message}");
            }
        }


    }
}
