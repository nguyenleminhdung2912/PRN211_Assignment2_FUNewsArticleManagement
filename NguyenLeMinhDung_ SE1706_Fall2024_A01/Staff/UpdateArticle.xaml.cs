using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for UpdateArticle.xaml
    /// </summary>
    public partial class UpdateArticle : Window
    {
        private NewsArticle currentArticle { get; set; }
        private SystemAccount currentAccount { get; set; }
        private ICategoryRepository categoryRepository { get; set; }
        private ITagRepository tagRepository { get; set; }
        private INewsArticleRepository newsArticleRepository { get; set; }
        private List<Category> categoryList { get; set; }
        private List<Tag> AvailableTagList { get; set; }
        private ICollection<Tag> SelectedTags { get; set; }
        //--------------------------------------------------------------------------------
        public UpdateArticle(NewsArticle newsArticle, SystemAccount systemAccount)
        {
            InitializeComponent();
            categoryRepository = new CategoryRepository();
            tagRepository = new TagRepository();
            newsArticleRepository = new NewsArticleRepository();
            this.currentArticle = newsArticle;
            this.currentAccount = systemAccount;
        }
        //--------------------------------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            LoadCategories();
            LoadTags();
        }
        //--------------------------------------------------------------------------------
        public void LoadCategories()
        {
            CategoryComboBox.ItemsSource = categoryRepository.GetCategories();
            CategoryComboBox.DisplayMemberPath = "CategoryName"; // Adjust this based on your role properties
            CategoryComboBox.SelectedValuePath = "CategoryId";
            CategoryComboBox.SelectedIndex = (int)currentArticle.CategoryId - 1;
        }
        public void LoadData()
        {
            UpdateArticleTable.DataContext = currentArticle;
        }
        public void LoadTags()
        {
            SelectedTags = currentArticle.Tags;
            AvailableTagList = tagRepository.GetTags();
            AvailableTagList = AvailableTagList
                .Where(tag => !SelectedTags.Any(selected => selected.TagId == tag.TagId))
                .ToList();
            AllTagsListBox.ItemsSource = AvailableTagList;
            SelectedTagsListBox.ItemsSource = SelectedTags;


        }
        //--------------------------------------------------------------------------------
        public void UpdateTheArticle()
        {
            // Lấy thông tin mới
            var updateArticle = currentArticle;

            //Validate
            if (NewsTitleTextBox.Text.IsNullOrEmpty() || HeadlineTextBox.Text.IsNullOrEmpty() || NewsContentTextBox.Text.IsNullOrEmpty() || NewsSourceTextBox.Text.IsNullOrEmpty() || CategoryComboBox.Text.IsNullOrEmpty() || SelectedTags.IsNullOrEmpty())
            {
                MessageBox.Show("Xin hãy điền đầy đủ thông tin!");
                return;
            }
            //Lấy category
            var category = CategoryComboBox.SelectedItem as Category;

            updateArticle.NewsTitle = NewsTitleTextBox.Text;
            updateArticle.Headline = HeadlineTextBox.Text;
            updateArticle.NewsContent = NewsContentTextBox.Text;
            updateArticle.NewsSource = NewsSourceTextBox.Text;
            updateArticle.Category = category;
            updateArticle.CategoryId = category.CategoryId;
            updateArticle.ModifiedDate = DateTime.Now;
            updateArticle.UpdatedById = currentAccount.AccountId;

            List<Tag> tags = (List<Tag>)updateArticle.Tags;

            newsArticleRepository.UpdateNewsArticleWithTags(updateArticle, tags);
            //newsArticleRepository.SaveNewsArticle(updateArticle); 
            MessageBox.Show("Cập nhật bài Article thành công!");
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
            UpdateTheArticle();
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
