using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ToursPage());
            Manager.MainFrame = MainFrame;

            //ImportTours();
        }

        private void ImportTours()
        {
            var fileData = File.ReadAllLines(@"..\Туры.txt");
            var images = Directory.GetFiles(@"..\Туры фото");

            foreach (var line in fileData)
            {
                var data = line.Split('\t');
                var tempTour = new Tour
                {
                    Name = data[0].Replace("\"", ""),
                    TicketsCount = int.Parse(data[2]),
                    Price = decimal.Parse(data[3]),
                    IsActual = (data[4] == "0") ? false : true
                };

                foreach (var tourType in data[5].Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentType = LilChaChaEntities.GetContext().Type.ToList().FirstOrDefault(p => p.Name == tourType);
                    if (currentType != null)
                    {
                        tempTour.Type.Add(currentType);
                    }
                }
                try
                {
                    tempTour.ImagePreveiw = File.ReadAllBytes(images.FirstOrDefault(p => p.Contains(tempTour.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                LilChaChaEntities.GetContext().Tour.Add(tempTour);
                LilChaChaEntities.GetContext().SaveChanges();
            }
        }

            private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if(MainFrame.CanGoBack) 
            {
                BtnBack.Visibility = Visibility.Visible;
            }
            else
            {
                BtnBack.Visibility= Visibility.Hidden;
            }
            if (Status.Auth == true)
                BtnLogin.Content = "Выйти";
            else
                BtnLogin.Content = "Войти";
            if(Status.Auth == false)
            {
                BtnEditHotel.Visibility = Visibility.Hidden;
                BtnEditTour.Visibility = Visibility.Hidden;
            }
            else
            {
                BtnEditHotel.Visibility = Visibility.Visible;
                if(Status.Admin == true)
                    BtnEditTour.Visibility = Visibility.Visible;
                else
                    BtnEditTour.Visibility = Visibility.Hidden;
            }
        }
        private void BtnLogin_Click(object obj, RoutedEventArgs e)
        {
            if (Status.Auth == true)
            {
                if (MessageBox.Show($"Вы точно хотите выйти из своей учётной записи ?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Status.Auth = false;
                    Status.Admin = false;
                    Status.Login = null;
                }
                MainFrame.Navigate(new ListView());
                MainFrame.Navigate(new ListView());
            }
            else
            {
                MainFrame.Navigate(new Auth());
            }
        }
        private void BtnEditHotel_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HotelPage());
        }
        private void BtnEditTour_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ToursPage());
        }
    }
}
