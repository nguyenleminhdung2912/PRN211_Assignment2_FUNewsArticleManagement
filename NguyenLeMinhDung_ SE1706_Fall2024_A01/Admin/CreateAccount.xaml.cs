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
using static NguyenLeMinhDung__SE1706_Fall2024_A01.Admin.AdminDashboard;

namespace NguyenLeMinhDung__SE1706_Fall2024_A01.Admin
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {

        public string AccountName { get; private set; }
        public string AccountEmail { get; private set; }
        public int AccountRole { get; private set; }
        public string AccountPassword { get; private set; }

        ISystemAccountRepository systemAccountRepository { get; set; }


        public CreateAccount(List<Role> roles)
        {
            InitializeComponent();
            systemAccountRepository = new SystemAccountRepository();
            AccountRoleComboBox.ItemsSource = roles;
            AccountRoleComboBox.DisplayMemberPath = "RoleName"; // Adjust this based on your role properties
            AccountRoleComboBox.SelectedValuePath = "RoleId";
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //Validate
            //Kiểm tra nếu không điền
            if (AccountNameTextBox.Text.IsNullOrEmpty() || AccountEmailTextBox.Text.IsNullOrEmpty() || AccountPasswordBox.Password.IsNullOrEmpty() || AccountRoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Xin hãy điền đầy đủ thông tin!");
                return;
            }
            ////Kiểm tra nếu email đúng định dạng
            //if (!AccountEmailTextBox.Text.EndsWith("@FUNewsManagement.org"))
            //{
            //    MessageBox.Show("Email phải kết thúc bằng @FUNewsManagement.org");
            //    return;
            //}
            //Kiểm tra nếu email đã tồn tại
            if (systemAccountRepository.GetSystemAccountByEmail(AccountEmailTextBox.Text) != null)
            {
                MessageBox.Show("Email này đã tồn tại trong hệ thống, xin hãy dùng email khác");
                return;
            }
            else
            {
                AccountName = AccountNameTextBox.Text;
                AccountEmail = AccountEmailTextBox.Text;
                AccountRole = (int)AccountRoleComboBox.SelectedValue; // Cast if needed
                AccountPassword = AccountPasswordBox.Password;

                DialogResult = true; // This will close the dialog and return true
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
