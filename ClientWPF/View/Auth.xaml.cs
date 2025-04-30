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
using ClientWPF.View;
using ClientWPF.VeiwModel;
namespace ClientWPF.View
{
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
            DataContext = new AuthViewModel();
        }
    }
}
