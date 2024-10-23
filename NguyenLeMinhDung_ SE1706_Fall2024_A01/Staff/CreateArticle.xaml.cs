using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NguyenLeMinhDung__SE1706_Fall2024_A01.Staff
{
    /// <summary>
    /// Interaction logic for CreateArticle.xaml
    /// </summary>
    public partial class CreateArticle : Window
    {

        private SystemAccount currentAccount { get; set; }
        private ICategoryRepository categoryRepository { get; set; }
        private ITagRepository tagRepository { get; set; }
        private INewsArticleRepository newsArticleRepository { get; set; }
        private List<Category> categoryList { get; set; }
        private List<Tag> AvailableTagList { get; set; }
        private ICollection<Tag> SelectedTags { get; set; } = new List<Tag>();
        public CreateArticle(SystemAccount systemAccount)
        {
            InitializeComponent();
            categoryRepository = new CategoryRepository();
            tagRepository = new TagRepository();
            newsArticleRepository = new NewsArticleRepository();
            this.currentAccount = systemAccount;
        }

        //--------------------------------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            LoadTags();
        }
        //--------------------------------------------------------------------------------
        public void LoadCategories()
        {
            CategoryComboBox.ItemsSource = categoryRepository.GetCategories();
            CategoryComboBox.DisplayMemberPath = "CategoryName"; // Adjust this based on your role properties
            CategoryComboBox.SelectedValuePath = "CategoryId";
        }
        public void LoadTags()
        {
            AvailableTagList = tagRepository.GetTags();
            AllTagsListBox.ItemsSource = AvailableTagList;
            SelectedTagsListBox.ItemsSource = SelectedTags;
        }
        //--------------------------------------------------------------------------------
        public void CreateTheArticle()
        {
            // Validate
            if (NewsTitleTextBox.Text.IsNullOrEmpty() || HeadlineTextBox.Text.IsNullOrEmpty() ||
                NewsContentTextBox.Text.IsNullOrEmpty() || NewsSourceTextBox.Text.IsNullOrEmpty() ||
                CategoryComboBox.Text.IsNullOrEmpty() || SelectedTags.IsNullOrEmpty())
            {
                MessageBox.Show("Xin hãy điền đầy đủ thông tin!");
                return;
            }

            NewsArticle createArticle = new NewsArticle();

            // Get category
            var category = CategoryComboBox.SelectedItem as Category;

            createArticle.NewsTitle = NewsTitleTextBox.Text;
            createArticle.Headline = HeadlineTextBox.Text;
            createArticle.NewsContent = NewsContentTextBox.Text;
            createArticle.NewsSource = NewsSourceTextBox.Text;
            //createArticle.Category = category;
            createArticle.CategoryId = category.CategoryId;
            createArticle.CreatedDate = DateTime.Now;
            createArticle.CreatedById = currentAccount.AccountId;
            //createArticle.CreatedBy = currentAccount;
            createArticle.ModifiedDate = DateTime.Now;
            createArticle.UpdatedById = currentAccount.AccountId;

            List<Tag> tags = (List<Tag>)SelectedTags;

            // Call to repository
            newsArticleRepository.CreateNewsArticleWithTags(createArticle, tags);

            newsArticleRepository.UpdateNewsArticleWithTags(createArticle, tags);
            MessageBox.Show("Create article successfully!");
            Close();
        }
        //--------------------------------------------------------------------------------
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //--------------------------------------------------------------------------------
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            CreateTheArticle();
        }
        //--------------------------------------------------------------------------------
        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllTagsListBox.SelectedItem is Tag selectedTag)
            {
                SelectedTags.Add(selectedTag);
                AvailableTagList.Remove(selectedTag);
                UpdateTagLists();
            }
        }
        //--------------------------------------------------------------------------------
        private void RemoveTagButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTagsListBox.SelectedItem is Tag selectedTag)
            {
                SelectedTags.Remove(selectedTag);
                AvailableTagList.Add(selectedTag);
                UpdateTagLists();
            }
        }
        //--------------------------------------------------------------------------------
        private void UpdateTagLists()
        {
            AllTagsListBox.ItemsSource = null;
            SelectedTagsListBox.ItemsSource = null;
            AllTagsListBox.ItemsSource = AvailableTagList;
            SelectedTagsListBox.ItemsSource = SelectedTags;
        }
    }
}
