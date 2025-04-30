using BusinessLayer.Services;
using ClientWPF.VeiwModel;
using System;
using System.Collections.Generic;
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

namespace ClientWPF.View
{
    public partial class AdminWindow : Window
    {
        public AdminWindow(string token)
        {
            InitializeComponent();
            var navigationService = new NavigationService();
            DataContext = new AdminViewModel(navigationService, token);
        }
    }
}
