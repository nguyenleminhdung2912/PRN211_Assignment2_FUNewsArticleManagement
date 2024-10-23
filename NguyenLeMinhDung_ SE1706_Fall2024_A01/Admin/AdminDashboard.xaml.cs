using BusinessObjects;
using Repository.IRepository;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using NguyenLeMinhDung__SE1706_Fall2024_A01;
using Microsoft.VisualBasic.Logging;
using Microsoft.IdentityModel.Tokens;

namespace NguyenLeMinhDung__SE1706_Fall2024_A01.Admin
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        ISystemAccountRepository systemAccountRepository { get; set; }
        INewsArticleRepository newsArticleRepository { get; set; }
        private List<SystemAccount> systemAccounts { get; set; }
        public List<Role> Roles { get; set; }
        //--------------------------------------------------------------------------------
        public AdminDashboard(MainWindow mainWindow)
        {
            InitializeComponent();
            systemAccounts = new List<SystemAccount>();
            systemAccountRepository = new SystemAccountRepository();
            newsArticleRepository = new NewsArticleRepository();
            LoadRoles();
        }
        //--------------------------------------------------------------------------------
        public class ToShow
        {
            public ToShow(short accountId, string? accountName, string? accountEmail, string accountRole, string accountPassword)
            {
                AccountId = accountId;
                AccountName = accountName;
                AccountEmail = accountEmail;
                AccountRole = accountRole;
                AccountPassword = accountPassword;
            }

            public short AccountId { get; set; }
            public string AccountName { get; set; }
            public string AccountEmail { get; set; }
            public string? AccountRole { get; set; }
            public string AccountPassword { get; set; }
        }
        public class Role
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }
        }
        private void AccountDetail()
        {
            if (AccountListView.SelectedItem != null)
            {
                // Perform actions based on the newly selected item
                SystemAccount selected = (SystemAccount)AccountListView.SelectedItem;
                string AccountRole = "";
                if (selected.AccountRole == 1)
                {
                    AccountRole = "Staff";
                    roleBox.Text = "Staff";
                }
                else if (selected.AccountRole == 2)
                {
                    AccountRole = "Lecturer";
                    roleBox.Text = "Lecturer";
                }

                string AccountPassword = "***";

                ToShow toShow = new(selected.AccountId, selected.AccountName, selected.AccountEmail, AccountRole, AccountPassword);
                DetailedAccount.DataContext = toShow;
            }
        }
        private void DisplayAccountDetail(object sender, SelectionChangedEventArgs e)
        {
            AccountDetail();
        }
        //--------------------------------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e) { LoadAccounts(); LoadRoles(); }
        //--------------------------------------------------------------------------------
        private void LoadAccounts()
        {
            systemAccounts = systemAccountRepository.GetSystemAccounts();
            AccountListView.ItemsSource = systemAccounts;
        }
        private void LoadRoles()
        {
            // Giả lập dữ liệu Role, thay thế bằng truy vấn từ database.
            Roles = new List<Role>
            {
                new Role { RoleId = 1, RoleName = "Staff" },
                new Role { RoleId = 2, RoleName = "Lecturer" },
            };
            roleBox.ItemsSource = Roles;
        }
        //--------------------------------------------------------------------------------
        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                DateTime startDate = StartDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;

                // Đảm bảo endDate lớn hơn startDate
                if (endDate < startDate)
                {
                    MessageBox.Show("End date must be greater than or equal to start date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kiểm tra xem endDate có lớn hơn ngày hiện tại hay không
                if (endDate > DateTime.Now || startDate > DateTime.Now)
                {
                    MessageBox.Show("End date and Start Date cannot be greater than today's date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lấy danh sách bài viết tin tức
                var articles = newsArticleRepository.GetNewsArticlesByDateRange(startDate, endDate);

                // Kiểm tra xem có dữ liệu nào không
                if (articles == null || articles.Count == 0)
                {
                    MessageBox.Show("No news articles found for the selected date range.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    NewsArticlesListView.ItemsSource = null; // Xóa nguồn dữ liệu nếu không có

                    // Cập nhật số lượng bài báo
                    ArticlesCountTextBlock.Text = "Total Articles: 0";
                    return;
                }

                // Cập nhật ListView
                NewsArticlesListView.ItemsSource = articles;
                // Hiển thị số lượng bài báo
                ArticlesCountTextBlock.Text = $"Total Articles: {articles.Count}";
            }
            else
            {
                MessageBox.Show("Please select both start and end dates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //--------------------------------------------------------------------------------
        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount(Roles);
            if (createAccount.ShowDialog() == true)
            {
                // If dialog returns true, add the new account to the database
                var newAccount = new SystemAccount
                {
                    AccountName = createAccount.AccountName,
                    AccountEmail = createAccount.AccountEmail,
                    AccountRole = createAccount.AccountRole,
                    AccountPassword = createAccount.AccountPassword,
                };
                // Add newAccount to the database
                systemAccountRepository.SaveAccount(newAccount);

                // Refresh the ListView
                LoadAccounts();
            }

        }
        //--------------------------------------------------------------------------------
        private void UpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            //Lấy thông tin từ DetailedAccount (Thông tin được update)
            string updateName = txtAccountName.Text;
            short updateAccountId = short.Parse(txtAccountId.Text);
            int role = 0;
            if (roleBox.Text == "Staff") role = 1;
            else if (roleBox.Text == "Lecturer") role = 2;
            string updateEmail = txtAccountEmail.Text;

            SystemAccount originalAccount = systemAccountRepository.GetSystemAccountById(updateAccountId);


            //Validate
            if (updateName.IsNullOrEmpty() || updateEmail.IsNullOrEmpty())
            {
                MessageBox.Show("Xin hãy điền đầy đủ thông tin!");
                AccountDetail();
                return;
            }
            //if (!updateEmail.EndsWith("@FUNewsManagement.org")) 
            //{
            //    MessageBox.Show("Email phải kết thúc bằng @FUNewsManagement.org");
            //    AccountDetail();
            //    return;
            //}
            if(!(updateEmail == originalAccount.AccountEmail) && systemAccountRepository.GetSystemAccountByEmail(updateEmail) != null)
            {
                MessageBox.Show("Email này đã tồn tại trong hệ thống, xin hãy dùng email khác");
                AccountDetail();
                return;
            }

            SystemAccount updateAccount = new SystemAccount()
            {
                AccountId = updateAccountId,
                AccountEmail = updateEmail,
                AccountName = updateName,
                AccountRole = role,
                AccountPassword = originalAccount.AccountPassword,
                NewsArticles = originalAccount.NewsArticles
            };

            if (updateAccount != null)
            {
                // Tiến hành cập nhật
                systemAccountRepository.UpdateAccount(updateAccount);
                LoadAccounts();
                AccountListView.SelectedItem = updateAccount;
                MessageBox.Show("Thông tin đã được thay đổi!");
            }
            else
            {
                MessageBox.Show("Hãy chọn một tài khoản để thay đổi");
            }
        }
        //--------------------------------------------------------------------------------
        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = AccountListView.SelectedItem as SystemAccount;

            if (selectedAccount != null)
            {
                // Xác nhận việc xóa tài khoản
                var result = MessageBox.Show("Bạn có chắc là muốn xoá tài khoản không?",
                                             "Bạn có chắc?",
                                             MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Xóa tài khoản khỏi danh sách
                    SystemAccount systemAccount = systemAccountRepository.GetSystemAccountByEmail(selectedAccount.AccountEmail);
                    if (!systemAccount.NewsArticles.IsNullOrEmpty())
                    {
                        MessageBox.Show("Tài khoản này không thể xoá do họ đã đăng bài!");
                        AccountDetail();
                        LoadAccounts();
                        return;
                    }
                    systemAccountRepository.DeleteAccount(selectedAccount);
                    AccountDetail();
                    MessageBox.Show("Tài khoản xoá thành công!");
                    LoadAccounts();
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn 1 tài khoản để xoá");
            }
        }
        //--------------------------------------------------------------------------------
    }
}
