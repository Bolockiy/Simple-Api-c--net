using ApiToDo.Infrastructure.Data;
using BusinessLayer.Services;
using Helper.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using toDoList.Entities.UserAccount;
using static System.Net.Mime.MediaTypeNames;

namespace ClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICrudService<UserAccount> _UserService;
        private readonly ServiceProvider _serviceProvider;

        public MainWindow()
        {
            InitializeComponent();
            var configuration = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false)
                      .Build();
            
            var services = new ServiceCollection();
            services.AddAppServices(configuration);
            services.AddBusinessLayer();
            _serviceProvider = services.BuildServiceProvider();
            _UserService = _serviceProvider.GetRequiredService<ICrudService<UserAccount>>();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void PlaceholderText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void BTN_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              var inBd = await _UserService.GetByUserNameAsync(PlaceholderText.Text);
                if (inBd!=null)
                {
                    MessageBox.Show("Такой пользовтаель уже существует в системе");
                    return;
                }
                UserAccount account = new UserAccount()
                {
                    UserName = PlaceholderText.Text,
                    FullName = PlaceholderText.Text,
                    Password = PlaceholderText_1.Text,
                    Role = 0
                };
               await _UserService.CreateAsync(account);
               MessageBox.Show("Вы успешно зарегестрировались");
               ChoiseWin choiseWin = new ChoiseWin();
               choiseWin.Show();
               this.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Что-то пошло не так {ex.Data.ToString()}" );
            }
       }
    }
}