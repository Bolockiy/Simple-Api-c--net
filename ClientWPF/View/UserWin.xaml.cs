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
    }
}
