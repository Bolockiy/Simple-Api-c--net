using ApiLayer.Extensions;
using ApiToDo.Domain.Entities;
using BusinessLayer.Services;
using ClientWPF.VeiwModel;
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
namespace ClientWPF.View
{
    public partial class TaskWin : Window
    {
        public TaskWin(string token)
        {
            InitializeComponent();
            DataContext = new TaskModel(token);           
        }
    }
}
