using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using NguyenLeMinhDung__SE1706_Fall2024_A01.Admin;
using Repository.IRepository;
using Repository.Repository;
using System.Windows;

namespace NguyenLeMinhDung__SE1706_Fall2024_A01.Staff
{
    /// <summary>
    /// Interaction logic for StaffDashboard.xaml
    /// </summary>
    public partial class StaffDashboard : Window
    {
        //--------------------------------------------------------------------------------
        ISystemAccountRepository systemAccountRepository;
        INewsArticleRepository newsArticleRepository;
        ITagRepository tagRepository;
        ICategoryRepository categoryRepository;
        //--------------------------------------------------------------------------------
        SystemAccount currentAccount;
        //--------------------------------------------------------------------------------
        public StaffDashboard(SystemAccount account)
        {
            InitializeComponent();
            systemAccountRepository = new SystemAccountRepository();
            newsArticleRepository = new NewsArticleRepository();
            tagRepository = new TagRepository();
            categoryRepository = new CategoryRepository();
            this.currentAccount = account;
        }
        //--------------------------------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHistoryNewsArticles();
            LoadAccountInfo();
            LoadArticles();
            LoadCategory();
        }
        //--------------------------------------------------------------------------------
        //Profiles
        public void LoadAccountInfo()
        {
            currentAccount = systemAccountRepository.GetSystemAccountById(currentAccount.AccountId);
            ProfilePanel.DataContext = currentAccount;
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateProfile updateProfile = new UpdateProfile(currentAccount);
            updateProfile.ShowDialog();
            LoadAccountInfo();
        }

        //--------------------------------------------------------------------------------
        //History
        public void LoadHistoryNewsArticles()
        {
            HistoryArticlesListView.ItemsSource = newsArticleRepository.GetNewsArticlesCreatedBy(currentAccount.AccountId);
        }
        //--------------------------------------------------------------------------------
        //Category management
        public void LoadCategory()
        {
            CategoriesListView.ItemsSource = categoryRepository.GetCategories();
        }
        private void CreateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            CreateCategory createCategory = new CreateCategory();
            createCategory.ShowDialog();
            LoadCategory();
        }

        private void UpdateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as Category;

            UpdateCategory updateCategory = new UpdateCategory(selectedCategory);
            updateCategory.ShowDialog();
            LoadCategory();
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as Category;

            if (selectedCategory != null)
            {
                // Xác nhận việc xóa tài khoản
                var result = MessageBox.Show("Bạn có chắc là muốn xoá category này không?",
                                             "Bạn có chắc?",
                                             MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Xóa tài khoản khỏi danh sách
                    Category category = categoryRepository.GetCategoryById(selectedCategory.CategoryId);
                    if (!category.NewsArticles.IsNullOrEmpty())
                    {
                        MessageBox.Show("Category này không thể xoá do đã có bài đăng!");
                        LoadCategory();
                        return;
                    }

                    category.IsActive = false;
                    categoryRepository.UpdateCategory(category);
                    MessageBox.Show("Category đã bị vô hiệu hoá!");
                    LoadCategory();
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn 1 category để xoá");
            }
        }

        private void SearchCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = txtCategorySearchTextBox.Text;
            if (searchText.IsNullOrEmpty())
            {
                LoadCategory();
                return;
            }
            CategoriesListView.ItemsSource = categoryRepository.GetCategorysContainName(searchText);
        }

        //--------------------------------------------------------------------------------
        //News management
        private void LoadArticles()
        {
            ArticlesListView.ItemsSource = newsArticleRepository.GetNewsArticles();
        }
        private void btnCreateArticle_Click(object sender, RoutedEventArgs e)
        {
            CreateArticle createArticle = new CreateArticle(currentAccount);
            createArticle.ShowDialog();
            LoadArticles();

        }
        private void btnDeleteArticle_Click(object sender, RoutedEventArgs e)
        {
            var selectedNewsArticle = ArticlesListView.SelectedItem as NewsArticle;

            if (selectedNewsArticle != null)
            {
                // Xác nhận việc xóa tài khoản
                var result = MessageBox.Show("Bạn có chắc là muốn xoá không?",
                                             "Bạn có chắc?",
                                             MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Chỉnh sửa active = false
                    selectedNewsArticle.NewsStatus = false;
                    newsArticleRepository.UpdateNewsArticle(selectedNewsArticle, null);
                    LoadArticles();
                    MessageBox.Show("Xoá bài báo thành công!");
                    LoadArticles();
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn 1 bài báo để xoá!");
            }
        }
        private void btnNewsSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text;
            if (searchText.IsNullOrEmpty()) 
            {
                LoadArticles();
                return;
            }
            ArticlesListView.ItemsSource = newsArticleRepository.GetNewsArticlesContainTitle(searchText);
        }
        private void btnUpdateArticle_Click(object sender, RoutedEventArgs e)
        {
            var selectedNewsArticle = ArticlesListView.SelectedItem as NewsArticle;
            UpdateArticle updateArticle = new UpdateArticle(selectedNewsArticle, currentAccount);
            updateArticle.ShowDialog();
            LoadArticles();
        }
        //--------------------------------------------------------------------------------

    }
}
