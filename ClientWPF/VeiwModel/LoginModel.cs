using ApiLayer.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ClientWPF.View;
using ClientWPF.VeiwModel.Base;

namespace ClientWPF.VeiwModel
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _login = "";
        private string _password = "";
        private string _statusMessage = "";
        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(nameof(Login)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand { get; }

        public AuthViewModel()
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            var result = await Connect.LoginAsync(Login, Password);
            if (result == null)
            {
                _statusMessage = "Неверный логин или пароль";
                return;
            }

            _statusMessage = $"Добро пожаловать, {result.UserName}!";

            Application.Current.Dispatcher.Invoke(() =>
            {
                Window? current = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
                Window next;

                if (result.isAdmin)
                    next = new AdminWin(result.AccessToken);
                else
                    next = new TaskWin(result.AccessToken);

                next.Show();
                current?.Close();
            });
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
 
}
