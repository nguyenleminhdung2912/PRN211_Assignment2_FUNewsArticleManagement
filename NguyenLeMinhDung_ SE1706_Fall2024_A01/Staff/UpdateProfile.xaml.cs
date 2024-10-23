using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Repository.Repository;
using System.Windows;

namespace NguyenLeMinhDung__SE1706_Fall2024_A01.Staff
{
    /// <summary>
    /// Interaction logic for UpdateProfile.xaml
    /// </summary>
    public partial class UpdateProfile : Window
    {
        ISystemAccountRepository systemAccountRepository { get; set; }
        private SystemAccount currentAccount;
        private SystemAccount updatedAccount;
        public UpdateProfile(SystemAccount account)
        {
            InitializeComponent();
            systemAccountRepository = new SystemAccountRepository();
            currentAccount = account;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProfileInfo();
        }
        public void LoadProfileInfo()
        {

            Profile.DataContext = currentAccount;
        }
        public void UpdateAccount()
        {
            // Lấy thông tin mới
            var updateAccount = currentAccount;
            updateAccount.AccountName = AccountNameTextBox.Text;
            updateAccount.AccountEmail = AccountEmailTextBox.Text;
            updateAccount.AccountPassword = AccountPasswordTextBox.Text;

            //Validate
            if (AccountNameTextBox.Text.IsNullOrEmpty() || AccountEmailTextBox.Text.IsNullOrEmpty() || AccountPasswordTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Xin hãy điền đầy đủ thông tin!");
                LoadProfileInfo();
                return;
            }
            if (updateAccount.AccountEmail != currentAccount.AccountEmail && systemAccountRepository.GetSystemAccountByEmail(updateAccount.AccountEmail) != null)
            {
                MessageBox.Show("Email này đã tồn tại trong hệ thống, xin hãy dùng email khác");
                LoadProfileInfo();
                return;
            }

            systemAccountRepository.UpdateAccount(updateAccount); // Adjust according to your DB access layer
            MessageBox.Show("Profile updated successfully!");
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAccount();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
