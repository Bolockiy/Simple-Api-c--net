using BusinessLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Shapes;
using toDoList.Entities.UserAccount;

namespace ClientWPF
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        private readonly ICrudService<UserAccount> _UserService;
        private readonly ServiceProvider _serviceProvider;
        public Auth()
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

        private async void BTN_1_Click(object sender, RoutedEventArgs e)
        {
            var inBd = await _UserService.GetByUserNameAsync(PlaceholderText.Text);
            if (inBd == null)
            {
                MessageBox.Show("Не нашлось пользователя с таким именем");
                return;
            }
            if (inBd.Role == 1)
            {
                AdminWin adminWin = new AdminWin();
                adminWin.Show();
                this.Close();
            }
            else
            {
                UserWin userWin = new UserWin();
                userWin.Show();
                this.Close();
            }
        }
    }
}
