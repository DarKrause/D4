using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Srez.Windows;
using Srez.Models;
using System.Security.Cryptography;


namespace Srez
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient { BaseAddress = new Uri(Properties.Settings.Default.BaseAdress) })
                {
                    var content = new StringContent("", Encoding.UTF8, "applocation/json");
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync($"/Login?login={TbLogin.Text}&password={ComputeSha256Hash(PbPassword.Password)}", content);
                    string token = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (TbLogin.Text == "fpurdy@mayert.com" && PbPassword.Password == "")
                    {
                        MessageBox.Show("Неверный логин или пароль");
                        return;
                    }

                    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ListReaders main = new ListReaders(token);
                        main.Show();
                        this.Close();
                    }
                    else MessageBox.Show("Пользователь не найден");


                }
            }
            catch 
            {
                
                MessageBox.Show("Неверный логин или пароль");
            }
           
        }

        private void ImgPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var a = PbPassword.Password;
            PbPassword.Visibility = Visibility.Hidden;
            TbPassword.Text = a;
            TbPassword.Visibility = Visibility.Visible;
        }

        private void ImgPassword_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var a = PbPassword.Password;
            PbPassword.Visibility = Visibility.Visible;
            TbPassword.Text = a;
            TbPassword.Visibility = Visibility.Hidden;
        }
        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sga256Hash = SHA256.Create())
            {
                byte[] bytes = sga256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i <bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
