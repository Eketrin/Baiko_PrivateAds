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

namespace Baiko_PrivateAds.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private Users _currentUser = new Users();
        public AddUserPage(Users selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null)
            {
                _currentUser = selectedUser;
                cmbRole.SelectedIndex = (int)_currentUser.ID_role-1;
            }
            else { cmbRole.SelectedIndex = 1; }
            DataContext = _currentUser;
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentUser.Login)) errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password)) errors.AppendLine("Укажите пароль!");
            if (string.IsNullOrWhiteSpace(_currentUser.Nickname)) errors.AppendLine("Укажите никнейм!");
            if ( (cmbRole.Text == "")) errors.AppendLine("Выберите роль!"); //(_currentUser.ID_role == null)
            else _currentUser.ID_role = cmbRole.SelectedIndex+1;
            // Проверяем переменную errors на наличие ошибок
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            // Добавляем в объект User новую запись
            if (_currentUser.ID_user == 0)
                Entities.GetContext().Users.Add(_currentUser);
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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AdminPage());
        }
    }
}
