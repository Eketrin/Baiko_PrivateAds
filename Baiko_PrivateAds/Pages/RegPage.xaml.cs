using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
	public partial class RegPage : Page
    {
        public RegPage()
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
        private void TBoxPasswordRepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtHintPasswordRepeat.Visibility = Visibility.Visible;
            if (TBoxPasswordRepeat.Password.Length > 0)
            {
                txtHintPasswordRepeat.Visibility = Visibility.Hidden;
            }
        }
        private void TBoxName_TextChanged(object sender, RoutedEventArgs e)
        {
            txtHintName.Visibility = Visibility.Visible;
            if (TBoxName.Text.Length > 0)
            {
                txtHintName.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBoxLogin.Text) || string.IsNullOrEmpty(TBoxPassword.Password) || string.IsNullOrEmpty(TBoxPasswordRepeat.Password) || string.IsNullOrEmpty(TBoxName.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            using (var pr = new Entities())
            {

                var user = pr.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TBoxLogin.Text);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }
            }

            if (TBoxPassword.Password.Length < 6)
            {
                MessageBox.Show("Пароль слишком короткий, минимум 6 символов");
                return;
            }

            bool en = false; // английская раскладка
            bool number = false; // цифра
            bool onlyEnglish = true; // только английские буквы
            for (int i = 0; i < TBoxPassword.Password.Length; i++) // перебираем
            {
                if (TBoxPassword.Password[i] >= 'А' && TBoxPassword.Password[i] <= 'Я') onlyEnglish = false; // если русская раскладка
                if (TBoxPassword.Password[i] >= '0' && TBoxPassword.Password[i] <= '9') number = true; // если цифра
                if ((TBoxPassword.Password[i] >= 'A' && TBoxPassword.Password[i] <= 'Z') 
                    || (TBoxPassword.Password[i] >= 'a' && TBoxPassword.Password[i] <= 'z')) en = true; // если английская буква  			
            }
            if (!onlyEnglish) // если найдены русские буквы
            {
                MessageBox.Show("Доступна только английская раскладка");
                return;
            }
            if (!en) // если нет английских букв
            {
                MessageBox.Show("Добавьте хотя бы одну английскую букву"); // выводим сообщение
                return;
            }
            if (!number)
            {
                MessageBox.Show("Добавьте хотя бы одну цифру"); // выводим сообщение
                return;
            }

            if (TBoxPassword.Password != TBoxPasswordRepeat.Password) // проверка на совпадение
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            Entities db = new Entities();
            Users userObject = new Users
            {
                Nickname = TBoxName.Text,
                Login = TBoxLogin.Text,
                Password = TBoxPassword.Password,
                ID_role = 2, 
                Profit = 0
           
            };
            db.Users.Add(userObject);
            db.SaveChanges();
            MessageBox.Show("Пользователь зарегистрирован!");
        
        }
 
    }
    public class Role
    {
        public string Name { get; set; }
    }
}
