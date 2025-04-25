using ApiLayer.Extensions;
using BusinessLayer.Services;
using Helper.Security;
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
        public Auth()
        {
            InitializeComponent();
            _UserService = App.ServiceProvider.GetRequiredService<ICrudService<UserAccount>>();
        }

        private async void BTN_1_Click(object sender, RoutedEventArgs e)
        {
            var result = await Connect.LoginAsync(LoginTextBox.Text, PasswordTextBox.Text);
           
            if (result == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            MessageBox.Show($"Добро пожаловать, {result.UserName}!");
            if (result.isAdmin)
            {
                AdminWin adminWin = new AdminWin(result.AccessToken);
                adminWin.Show();
                this.Close();
            }
            else
            {
                TaskWin taskWin = new TaskWin(result.AccessToken);
                taskWin.Show();
                this.Close();
            }
        }
    }
}
