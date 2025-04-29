using BusinessLayer.Services;
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

    /// <summary>
    /// Логика взаимодействия для AdminWin.xaml
    /// </summary>
    public partial class AdminWin : Window
    {
        private readonly string Token;
        public AdminWin(string token)
        {
            InitializeComponent();
            Token = token;
        }

        private void OpenTaskWindow_Click(object sender, RoutedEventArgs e)
        {
            var taskWindow = new TaskWin(Token);
            taskWindow.Show();
            this.Close();
        }

        private void OpenUserWindow_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new UserWin(Token);
            userWindow.Show();
            this.Close();
        }
    }
}
