using ApiLayer.Extensions;
using ApiToDo.Domain.Entities;
using BusinessLayer.Services;
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
        private readonly ICrudService<ToDoTask> _taskService;
        private readonly string Token;

        public TaskWin(string token)
        {
            InitializeComponent();
            _taskService = App.ServiceProvider.GetRequiredService<ICrudService<ToDoTask>>();
            Token = token;
            AddTasksAsync();
           
        }

        public async void AddTasksAsync()
        {
            var tasks = await Connect.GetTasksAsync(Token);
            TasksDataGrid.ItemsSource = tasks;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newTask = new ToDoTask()
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    IsCompleted = IsCompletedCheckBox.IsChecked == true
                };

                var result = await Connect.CreateTaskAsync(newTask, Token);
                if (result)
                {
                    MessageBox.Show("Задача успешно добавлена");
                    AddTasksAsync();
                }
                else
                {
                    MessageBox.Show("Не удалось добавить задачу");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(TaskIdTextBox.Text, out int taskId))
                {
                    var task = await Connect.GetTaskByIdAsync(taskId, Token);
                    if (task != null)
                    {
                        var result = await Connect.DeleteTaskAsync(taskId, Token);
                        if (result)
                        {
                            MessageBox.Show("Задача успешно удалена");
                            AddTasksAsync();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить задачу");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Задача с таким ID не найдена");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректный ID задачи");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(TaskIdTextBox.Text, out int taskId))
                {
                    var task = await Connect.GetTaskByIdAsync(taskId, Token);
                    if (task != null)
                    {
                        task.Title = TitleTextBox.Text;
                        task.Description = DescriptionTextBox.Text;
                        task.IsCompleted = IsCompletedCheckBox.IsChecked == true;

                        var result = await Connect.UpdateTaskAsync(taskId, task, Token);
                        if (result)
                        {
                            MessageBox.Show("Задача успешно обновлена");
                            AddTasksAsync();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить задачу");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Задача с таким ID не найдена");
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректный ID задачи");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            AddTasksAsync();
        }
    }
}
