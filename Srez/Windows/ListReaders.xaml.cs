using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Srez.Models;
using word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.IO;

namespace Srez.Windows
{
    /// <summary>
    /// Логика взаимодействия для ListReaders.xaml
    /// </summary>
    public partial class ListReaders : System.Windows.Window
    {
        public string token;
        public ListReaders(string token)
        {
            InitializeComponent();
            this.token = token;
        }
        public List<Reader> readers = new List<Reader>();


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (HttpClient httpClient = new HttpClient { BaseAddress = new Uri(Properties.Settings.Default.BaseAdress) })
            {
                var content = new StringContent("", Encoding.UTF8, "applocation/json");
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"/GetReaders?token={token}");
                string responceBody = await httpResponseMessage.Content.ReadAsStringAsync();
                readers = JsonSerializer.Deserialize<List<Reader>>(responceBody);
                LvReaders.ItemsSource = readers;

            }
        }

        private void TbReaders_TextChanged(object sender, TextChangedEventArgs e)
        {
            LvReaders.ItemsSource = readers.Where(p => p.fio.Contains(TbReaders.Text)).ToList();
        }

        private void ImgBilet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files(*.docx)|*.docx|All files(*.*)|*.*";
            string source = $@"{Directory.GetCurrentDirectory()}\читательский_билет_template.docx";
            word.Application app = new word.Application();
            word.Document doc = app.Documents.Open(source);
            doc.Activate();

            try
            {
                if (sfd.ShowDialog() == false)
                {
                    doc.Close();
                    doc = null;
                    app.Quit();
                    return;
                }

                doc.SaveAs2(sfd.FileName);
                doc.Close();
                doc = null;
                app.Quit();
                MessageBox.Show("Файл успешно создан");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                doc.Close();
                doc = null;
                app.Quit();
            }
        }
    }
}
