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

namespace WPFStudyPractice
{
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
            Login.FontSize = Login.Height - 10;
            Password.FontSize = Password.Height - 10;
        }
        
        private void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            bool b = false;
            var aut = LilChaChaEntities.GetContext().Users.ToList();
            for (int i = 0; i < aut.Count; i++)
            {
                if (aut[i].Login.ToString() == Login.Text && aut[i].Password.ToString() == Password.Text)
                {
                    Status.Auth = true;
                    if (aut[i].Admin == true)
                    {
                        Status.Admin = true;
                    }
                    b = true;
                    MessageBox.Show("Авторизация прошла успешно!");
                    Manager.MainFrame.GoBack();
                    break;
                }
            }
            if (b == false)
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }
        }
    }
}
