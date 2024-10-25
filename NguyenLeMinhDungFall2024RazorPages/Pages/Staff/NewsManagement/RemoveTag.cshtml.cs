using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.NewsManagement
{
    public class RemoveTagModel : PageModel
    {
        private readonly ITagRepository tagRepository;
        private readonly INewsArticleRepository newsArticleRepository;
        private readonly IHubContext<SignalRHub> hubContext;

        public RemoveTagModel(IHubContext<SignalRHub> hubContext)
        {
            newsArticleRepository = new NewsArticleRepository();
            tagRepository = new TagRepository();
            this.hubContext = hubContext;

        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<Tag> AvailableTags { get; set; } = new List<Tag>();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();
        private ICollection<Tag> SelectedTags { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var newsarticle = newsArticleRepository.GetNewsArticleById(id);

            NewsArticle = newsarticle;

            if (newsarticle == null)
            {
                return NotFound();
            }

            // Load available tags
            SelectedTags = newsarticle.Tags;

            var AvailableTags = tagRepository.GetTags();
            AvailableTags = AvailableTags
            .Where(tag => SelectedTags.Any(selected => selected.TagId == tag.TagId))
            .ToList();

            ViewData["Tags"] = new SelectList(AvailableTags, "TagId", "TagName");
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                NewsArticle currentArticle = newsArticleRepository.GetNewsArticleById(NewsArticle.NewsArticleId);
                List<int> tagIdsToRemove = SelectedTagIds; { /* tag IDs to remove */ };

                newsArticleRepository.RemoveTag(currentArticle, tagIdsToRemove);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsArticleExists(NewsArticle.NewsArticleId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await hubContext.Clients.All.SendAsync("RefreshData");

            return RedirectToPage("./Index");
        }

        private bool NewsArticleExists(string id)
        {
            return newsArticleRepository.GetNewsArticleById(id) == null ? true : false;
        }
    }
}
