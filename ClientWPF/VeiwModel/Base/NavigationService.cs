using ClientWPF.VeiwModel.Base;
using ClientWPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientWPF.VeiwModel
{
    public class NavigationService : INavigationService
    {
        public void NavigateToTask(string token)
        {
            var window = new TaskWindow(token);
            ShowAndCloseCurrent(window);
        }

        public void NavigateToUser(string token)
        {
            var window = new UserWindow(token);
            ShowAndCloseCurrent(window);
        }

        private void ShowAndCloseCurrent(Window newWindow)
        {
            Window? current = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            newWindow.Show();
            current?.Close();
        }
    }

}
