using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
        int userData;
        private Entities context;
        public UserPage(int userdata)
        {
            InitializeComponent();
            SortComboBox.SelectedIndex = 0;

            var currentPosts = Entities.GetContext().Posts.ToList();
            
            ListUser.ItemsSource = currentPosts;
            userData = userdata;
            UpdatePosts();

        }

        private void UpdatePosts()
        {
            //загружаем всех пользователей в список
            var currentPosts = Entities.GetContext().Posts.ToList();
            //осуществляем поиск по названию поста без учета регистра букв
            currentPosts = currentPosts.Where(x => x.Name_post.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();
            if (RoleCheckBox.IsChecked.Value)
            { currentPosts = currentPosts.Where(x => (int)x.ID_user == userData).ToList(); }
            else { currentPosts = currentPosts.Where(x => x.ID_status == 2).ToList(); }
            //осуществляем сортировку в зависимости от выбора пользователя
            if (SortComboBox.SelectedIndex == 0)
                ListUser.ItemsSource = currentPosts.OrderBy(x => x.Name_post).ToList();
            else
                ListUser.ItemsSource = currentPosts.OrderByDescending(x => x.Name_post).ToList();
            var profit = Entities.GetContext().Users
                        .Where(x => (int)x.ID_user == userData)
                        .Select(x => (int)x.Profit)
                        .FirstOrDefault();
            prof.Text = profit.ToString();



        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListUser.SelectedItem != null && ListUser.SelectedItem is Posts selectedPost)
            {
                if (RoleCheckBox.IsChecked.Value)
                {
                    NavigationService?.Navigate(new Pages.PostDetailPage(selectedPost, userData, false));
                }
                else NavigationService?.Navigate(new Pages.PostDetailPage(selectedPost, -1, false));

            }
        }

        private void RoleCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            DelButton.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Visible;
            UpdatePosts();
        }
        private void RoleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DelButton.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            UpdatePosts();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePosts();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            NavigationService?.Navigate(new Pages.PostDetailPage(null, userData, true));
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var postsForRemoving = ListUser.SelectedItems.Cast<Posts>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {postsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                try
                {
                    Entities.GetContext().Posts.RemoveRange(postsForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены");

                    UpdatePosts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
        }
    }
}
