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
namespace ClientWPF
{
    public partial class TaskWin : Window
    {
        private readonly ICrudService<ToDoTask> _taskService;

        public TaskWin()
        {
            InitializeComponent();
            _taskService = App.ServiceProvider.GetRequiredService<ICrudService<ToDoTask>>();
            AddTasksAsync();
        }

        public async void AddTasksAsync()
        {
            var tasks = await _taskService.GetAllAsync();
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

                var result = await _taskService.CreateAsync(newTask);
                if (result)
                {
                    MessageBox.Show("Задача успешно добавлена");
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
                var idStr = TaskIdTextBox.Text;
                if (int.TryParse(idStr, out int taskId))
                {
                    var task = await _taskService.GetByIdAsync(taskId);
                    if (task != null)
                    {
                        await _taskService.DeleteAsync(taskId);
                        MessageBox.Show("Задача успешно удалена");
                    }
                    else
                    {
                        MessageBox.Show("Задача с таким ID не найдена");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректный ID задачи для удаления");
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
                var idStr = TaskIdTextBox.Text;
                if (int.TryParse(idStr, out int taskId))
                {
                    var task = await _taskService.GetByIdAsync(taskId);
                    if (task != null)
                    {
                        task.Title = TitleTextBox.Text;
                        task.Description = DescriptionTextBox.Text;
                        task.IsCompleted = IsCompletedCheckBox.IsChecked == true;

                        var result = await _taskService.UpdateAsync(taskId, task);
                        if (result)
                        {
                            MessageBox.Show("Задача успешно обновлена");
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
                    MessageBox.Show("Пожалуйста, введите корректный ID задачи для обновления");
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
