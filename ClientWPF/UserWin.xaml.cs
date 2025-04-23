using BusinessLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using toDoList.Entities.UserAccount;

namespace ClientWPF
{
    public partial class UserWin : Window
    {
        private readonly ICrudService<UserAccount> _userService;
        public UserWin()
        {
            InitializeComponent();
            _userService = App.ServiceProvider.GetRequiredService<ICrudService<UserAccount>>();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации пользователя: {ex.Message}");
            }

        }

        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = UserNameTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(userName))
                {
                    MessageBox.Show("Введите имя пользователя для удаления.");
                    return;
                }

                var user = await _userService.GetByUserNameAsync(userName);
                if (user != null)
                {
                    await _userService.DeleteAsync(user.Id);
                    MessageBox.Show("Пользователь успешно удален.");
                }
                else
                {
                    MessageBox.Show("Пользователь с таким именем не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}");
            }
        }

        private async void GetUserByNameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = UserNameTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(userName))
                {
                    MessageBox.Show("Введите имя пользователя для поиска.");
                    return;
                }
                var user = await _userService.GetByUserNameAsync(userName);
                if (user != null)
                {
                    MessageBox.Show($"Пользователь найден: {user.FullName}");
                }
                else
                {
                    MessageBox.Show("Пользователь с таким именем не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении пользователя: {ex.Message}");
            }
        }

    }
}
