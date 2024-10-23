using BusinessObjects;
using Microsoft.Identity.Client;
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
    /// Interaction logic for UpdateCategory.xaml
    /// </summary>
    public partial class UpdateCategory : Window
    {

        Category currentCategory = new Category();
        ICategoryRepository categoryRepository;

        public UpdateCategory(Category category)
        {
            InitializeComponent();
            this.currentCategory = category;
            categoryRepository = new CategoryRepository();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            UpdateCategoryView.DataContext = currentCategory;
        }
        public void UpdateTheCategory()
        {
            // Lấy thông tin mới
            var updateCategory = currentCategory;

            //Validate
            if (txtCategoryName.Text.IsNullOrEmpty() || txtDescription.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Xin hãy điền đầy đủ thông tin!");
                return;
            }
            //Lấy category

            updateCategory.CategoryName = txtCategoryName.Text;
            updateCategory.CategoryDesciption = txtDescription.Text;


            categoryRepository.UpdateCategory(updateCategory);
            MessageBox.Show("Cập nhật Category thành công!");
            Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateTheCategory();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
