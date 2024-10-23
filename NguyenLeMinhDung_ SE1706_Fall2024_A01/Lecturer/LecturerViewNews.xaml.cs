using BusinessObjects;
using Repository.IRepository;
using Repository.Repository;
using System;
using System.Windows;

namespace NguyenLeMinhDung__SE1706_Fall2024_A01.Lecturer
{
    /// <summary>
    /// Interaction logic for LecturerViewNews.xaml
    /// </summary>
    public partial class LecturerViewNews : Window
    {
        INewsArticleRepository newsArticleRepository { get; set; }

        public List<NewsArticle> newsArticles { get; set; }

        public LecturerViewNews()
        {
            InitializeComponent();
            newsArticles = new List<NewsArticle>();
            newsArticleRepository = new NewsArticleRepository();
        }
        //--------------------------------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e) => LoadArticles();
        //--------------------------------------------------------------------------------
        private void LoadArticles()
        {
            ArticlesListView.ItemsSource = newsArticleRepository.GetNewsArticles();
        }
    }
}
