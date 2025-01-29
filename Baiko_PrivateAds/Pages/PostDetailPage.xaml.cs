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
using System.Windows.Threading;

namespace Baiko_PrivateAds.Pages
{
    /// <summary>
    /// Логика взаимодействия для PostDetailPage.xaml
    /// </summary>
    public partial class PostDetailPage : Page
    {
        private Posts _currentPost = new Posts();
        private Users _currentUsers = new Users();
        private Entities context;
        bool _addPost ;
        public PostDetailPage(Posts selectedPost, int isMy, bool addPost)
        {
            InitializeComponent();
            context = new Entities();
            _addPost = addPost;
            if (selectedPost != null) //перенос выбранного поста
            {
                _currentPost = selectedPost;
                if (isMy != -1 && selectedPost.ID_status != 3) // если пост мой редачим
                {
                    setEdit();
                    SaveBut.Visibility = Visibility.Visible;
                    SoldBut.Visibility = Visibility.Visible;
                }
            }
            
            if (addPost) //если нужно добавить пост
            {
                _currentPost.ID_status = 2;
                _currentPost.ID_user = isMy;
                boxStatus.Visibility = Visibility.Hidden;
                stat.Visibility = Visibility.Hidden;
                _currentPost.Date_public = DateTime.Now;
                AddBut.Visibility = Visibility.Visible;
                setEdit();
            }
            
            DataContext = _currentPost;

        }

        private void setEdit()
        {
            box2.IsReadOnly = false;
            box3.IsReadOnly = false;
            box4.IsReadOnly = false;
            box5.IsReadOnly = false;
            box6.IsReadOnly = false;
            box7.IsReadOnly = false;
            box8.IsReadOnly = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentPost.Category)) errors.AppendLine("Укажите категорию!");
            if (string.IsNullOrWhiteSpace(_currentPost.Type)) errors.AppendLine("Укажите тип товара!");
            if (string.IsNullOrWhiteSpace(_currentPost.Name_post)) errors.AppendLine("Напишите название поста!");
            if (string.IsNullOrWhiteSpace(_currentPost.City)) errors.AppendLine("Укажите город!");
            if (string.IsNullOrWhiteSpace((_currentPost.Cost).ToString())) errors.AppendLine("Укажите цену!");
            if (string.IsNullOrWhiteSpace(_currentPost.Description)) errors.AppendLine("Напишите описание!");
            
            // Проверяем переменную errors на наличие ошибок
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            // Добавляем в объект Post новую запись
            if (_currentPost.ID_post == 0)
                Entities.GetContext().Posts.Add(_currentPost);
            // Делаем попытку записи данных в БД о новом пользователе
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void SoldBut_Click(object sender, RoutedEventArgs e)
        {
            _currentUsers = Entities.GetContext().Users.FirstOrDefault(x => x.ID_user == _currentPost.ID_user);
            _currentUsers.Profit += _currentPost.Cost;

            // .Where(x => x.ID_user == _currentPost.ID_user).

            //закрываем пост
            _currentPost.ID_status = 3;
            // Добавляем в объект Post новую запись
            if (_currentPost.ID_post == 0)
                Entities.GetContext().Posts.Add(_currentPost);
                
            // Делаем попытку записи данных в БД о новом пользователе
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Пост закрыт!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }



    }
}
