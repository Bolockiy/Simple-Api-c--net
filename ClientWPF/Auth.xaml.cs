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
            var inBd = await _UserService.GetByUserNameAsync(LoginTextBox.Text);
            if (inBd == null)
            {
                MessageBox.Show("Не нашлось пользователя с таким именем");
                return;
            }

            if (!PasswordHasher.VerifyPassword(PasswordTextBox.Text, inBd.Password))
            {
                MessageBox.Show("Неверный пароль");
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
                TaskWin taskWin = new TaskWin();
                taskWin.Show();
                this.Close();
            }
        }
    }
}
