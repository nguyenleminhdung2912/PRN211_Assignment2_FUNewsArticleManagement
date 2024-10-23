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
    /// Interaction logic for CreateCategory.xaml
    /// </summary>
    public partial class CreateCategory : Window
    {

        ICategoryRepository categoryRepository;
        public CreateCategory()
        {
            InitializeComponent();
            categoryRepository = new CategoryRepository();

        }

        public void CreateTheCategory()
        {
            //Validate
            if (txtCategoryName.Text.IsNullOrEmpty() || txtDescription.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Xin hãy điền đầy đủ thông tin!");
                return;
            }
            //Lấy category
            Category newCategory = new Category();
            newCategory.CategoryName = txtCategoryName.Text;
            newCategory.CategoryDesciption = txtDescription.Text;
            newCategory.IsActive = true;

            categoryRepository.SaveCategory(newCategory);
            
            MessageBox.Show("Lưu category thành công!");
            Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            CreateTheCategory();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
