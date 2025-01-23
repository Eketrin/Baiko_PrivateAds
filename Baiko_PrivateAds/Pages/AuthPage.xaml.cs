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

namespace Baiko_PrivateAds.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void TBoxLogin_TextChanged(object sender, RoutedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (TBoxLogin.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void TBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Visible;
            if (TBoxPassword.Password.Length > 0)
            {
                txtHintPassword.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBoxLogin.Text) || string.IsNullOrEmpty(TBoxPassword.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }
            
            using (var db = new Entities())
            {
                var user = db.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TBoxLogin.Text && u.Password == TBoxPassword.Password);
                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                    return;
                }
                MessageBox.Show("Пользователь успешно найден!");

                switch (user.ID_role)
                {
                    case 1:     // "Администратор":
                        NavigationService?.Navigate(new AdminPage());
                        break;
                    case 2:     //"Пользователь":
                        NavigationService?.Navigate(new UserPage());
                        break;
                    case 3:     // "Модератор":
                        NavigationService?.Navigate(new ModPage());
                        break;
                }
            }
        }
        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
    }
}
