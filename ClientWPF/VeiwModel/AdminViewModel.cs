using ClientWPF.VeiwModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientWPF.VeiwModel
{
    public class AdminViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly string _token;

        public ICommand OpenTaskCommand { get; }
        public ICommand OpenUserCommand { get; }

        public AdminViewModel(INavigationService navigationService, string token)
        {
            _navigationService = navigationService;
            _token = token;

            OpenTaskCommand = new RelayCommand(() => _navigationService.NavigateToTask(_token));
            OpenUserCommand = new RelayCommand(() => _navigationService.NavigateToUser(_token));
        }
    }

}
