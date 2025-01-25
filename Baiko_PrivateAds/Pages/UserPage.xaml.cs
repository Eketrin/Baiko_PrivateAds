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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Baiko_PrivateAds.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            SortComboBox.SelectedIndex = 0;

            var currentPosts = Entities.GetContext().Posts.ToList();
            ListUser.ItemsSource = currentPosts;
        }

        private void UpdatePosts()
        {
            //загружаем всех пользователей в список
            var currentUsers = Entities.GetContext().Posts.ToList();
            //осуществляем поиск по названию поста без учета регистра букв
            currentUsers = currentUsers.Where(x => x.Name_post.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();
            //осуществляем сортировку в зависимости от выбора пользователя
            if (SortComboBox.SelectedIndex == 0)
                ListUser.ItemsSource = currentUsers.OrderBy(x => x.Name_post).ToList();
            else
                ListUser.ItemsSource = currentUsers.OrderByDescending(x => x.Name_post).ToList();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListUser.SelectedItem != null && ListUser.SelectedItem is Posts selectedPost)
            {
                NavigationService?.Navigate(new Pages.PostDetailPage(selectedPost));
                //NavigationService.Navigate(new Pages.AddUserPage((sender as Button).DataContext as Users));

            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePosts();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePosts();
        }
        private void SortCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePosts();
        }

        private void RoleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdatePosts();
        }

        private void RoleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdatePosts();
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            SortComboBox.SelectedIndex = 0;
            UpdatePosts();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
