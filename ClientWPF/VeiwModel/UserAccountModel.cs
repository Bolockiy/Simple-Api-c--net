using ApiLayer.Extensions;
using BusinessLayer.Model;
using BusinessLayer.Services;
using ClientWPF.VeiwModel.Base;
using ClientWPF.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using toDoList.Entities.UserAccount;

namespace ClientWPF.VeiwModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string _userName = "";
        private string _statusMessage = "";
        private readonly string _token;
        private string _password = "";
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(nameof(UserName)); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(_password)); }
        }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand CreateCommand { get;}
        public ICommand GetCommand { get; }

        public UserViewModel(string token)
        {
            _token = token;
            DeleteCommand = new RelayCommand(DeleteUser);
            CreateCommand = new RelayCommand(CreateUser);
            UpdateCommand = new RelayCommand(UpdateUser);
            GetCommand = new RelayCommand(GetUser);
        }

        private async void DeleteUser()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                StatusMessage = "Введите имя пользователя.";
                return;
            }

            try
            {
                var user = await Connect.GetUserByNameAsync(UserName, _token);
                if (user != null)
                {
                    var result = await Connect.DeleteUserAsync(user.Id, _token);
                    if (result)
                        StatusMessage = "Пользователь успешно удален.";
                    else
                        StatusMessage = "Не удалось удалить пользователя.";
                }
                else
                {
                    StatusMessage = "Пользователь с таким именем не найден.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при удалении пользователя: {ex.Message}";
            }
        }

        private async void CreateUser()
        {
            try
            {
                UserAccount account = new UserAccount()
                {
                    UserName = UserName,
                    FullName = UserName,
                    Password = _password,
                    Role = 0
                };
                await Connect.CreateUserAsync(account, _token);
                StatusMessage = "Вы успешно зарегестрировались";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Что-то пошло не так {ex.Data.ToString()}";
            }
        }

        private async void UpdateUser()
        {
           // await ;
        }
        private async void GetUser()
        {
           // await;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
