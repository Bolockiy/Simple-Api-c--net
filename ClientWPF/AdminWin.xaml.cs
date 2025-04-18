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

namespace ClientWPF
{
    /// <summary>
    /// Логика взаимодействия для AdminWin.xaml
    /// </summary>
    public partial class AdminWin : Window
    {
        public AdminWin()
        {
            InitializeComponent();
        }

        private void OpenTaskWindow_Click(object sender, RoutedEventArgs e)
        {
            var taskWindow = new TaskWin();
            taskWindow.Show();
            this.Close();
        }

        private void OpenUserWindow_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new UserWin();
            userWindow.Show();
            this.Close();
        }
    }
}
