using BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toDoList.Entities.UserAccount;

namespace ClientWPF.VeiwModel
{
    internal class UserViewModel : INotifyPropertyChanged
    {
        private readonly ICrudService<UserAccount> _userService;
        private readonly string _userName = "";
        private readonly string _token = "";
        public event PropertyChangedEventHandler? PropertyChanged;

        public UserViewModel(ICrudService<UserAccount> userService, string token)
        {

            _userService = userService;
            _token = token;

        }
        protected void onPropertyChanged(string? name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
