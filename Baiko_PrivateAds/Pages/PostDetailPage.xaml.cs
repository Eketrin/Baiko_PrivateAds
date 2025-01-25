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
    /// Логика взаимодействия для PostDetailPage.xaml
    /// </summary>
    public partial class PostDetailPage : Page
    {
        public PostDetailPage(Posts selectedPost)
        {
            InitializeComponent();
            DataContext = selectedPost;
        }

 
    }
}
