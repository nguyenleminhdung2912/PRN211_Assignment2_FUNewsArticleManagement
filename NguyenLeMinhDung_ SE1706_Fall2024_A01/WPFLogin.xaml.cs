using System.Configuration;
using System.IO;
using System.Windows;
using BusinessObjects;
using DataAccessObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using NguyenLeMinhDung__SE1706_Fall2024_A01.Admin;
using NguyenLeMinhDung__SE1706_Fall2024_A01.Lecturer;
using NguyenLeMinhDung__SE1706_Fall2024_A01.Staff;

namespace NguyenLeMinhDung__SE1706_Fall2024_A01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public IConfigurationRoot Configuration { get; set; }

        private void btnSignin_Click(object sender, RoutedEventArgs e)
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = config.Build();
            var adminUser = Configuration["AdminAccount:Email"];
            var adminPassword = Configuration["AdminAccount:Password"];

            string loginUsername = txtEmail.Text;
            string loginPassword = txtPassword.Password;

            if (adminUser == loginUsername && adminPassword == loginPassword)
            {
                AdminDashboard adminDashboard = new AdminDashboard(this);
                adminDashboard.Show();
            }
            else if (SystemAccountDAO.CheckLogin(loginUsername, loginPassword) != null)
            {
                SystemAccount account = SystemAccountDAO.CheckLogin(loginUsername, loginPassword);
                if (account.AccountRole.Equals(1)) 
                {
                    //Mở trang staff
                    StaffDashboard staffDashboard = new StaffDashboard(account);
                    staffDashboard.Show();
                    //this.Hide();
                }
                else if(account.AccountRole.Equals(2))
                {
                    //Mở trang lecturer
                    LecturerViewNews lecturerViewNews = new();
                    lecturerViewNews.Show();
                    //this.Hide();
                }
            }
            else
            {
                MessageBox.Show("You have no permission to do this function!");
            }
        }
    }
}