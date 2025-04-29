using ApiLayer.Extensions;
using ApiToDo.Domain.Entities;
using ClientWPF.VeiwModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace ClientWPF.View
{
    public partial class UserWin : Window
    {
        private string Token;
        public UserWin(string token)
        {
            Token = token;
            InitializeComponent();
            DataContext = new UserViewModel(token); 
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

                var user = await Connect.GetUserByNameAsync(userName, Token);
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

        private void UserNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
