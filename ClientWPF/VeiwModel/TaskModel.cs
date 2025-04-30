using ApiLayer.Extensions;
using ApiToDo.Domain.Entities;
using BusinessLayer.Model;
using BusinessLayer.Services;
using ClientWPF.VeiwModel.Base;
using ClientWPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using toDoList.Entities.UserAccount;

namespace ClientWPF.VeiwModel
{
    public class TaskModel : INotifyPropertyChanged
    {
        private string _id = "";
        private string _Tittle = "";
        private string _statusMessage = "";
        private readonly string _token;
        private string _description = "";
        private bool _isCompleted = false;
        private DateTime _CreatedAt = DateTime.UtcNow;
        private DateTime _UpdatedAt = DateTime.UtcNow;
        public ObservableCollection<ToDoTask> Tasks { get; set; } = new();
        public async Task LoadTasksAsync()
        {
            Tasks.Clear();
            var tasksFromDb = await Connect.GetTasksAsync(_token);
            foreach (var task in tasksFromDb)
                Tasks.Add(task);
        }

        public string Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Tittle
        {
            get => _Tittle;
            set { _Tittle = value; OnPropertyChanged(nameof(Tittle)); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(nameof(IsCompleted)); }
        }

        public DateTime CreatedAt
        { 
            get => _CreatedAt;
            set { _CreatedAt = value;OnPropertyChanged(nameof(_CreatedAt)); }
        }
        public DateTime UpdatedAt
        { 
            get => _UpdatedAt;
            set { _UpdatedAt = value;OnPropertyChanged(nameof(_UpdatedAt)); }
        }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand CreateCommand { get;}
        public ICommand UpdateTasksCommand { get; }

        public TaskModel(string token)
        {
            _token = token;
            DeleteCommand = new RelayCommand(DeleteTask);
            CreateCommand = new RelayCommand(CreateTask);
            UpdateCommand = new RelayCommand(UpdateTask);
            UpdateTasksCommand = new RelayCommand(UpdateTasks);
        }

        private async void DeleteTask()
        {
            try
            {
                int id = 0;
                if (int.TryParse(_id, out id))
                {
                    var task = await Connect.GetTaskByIdAsync(id, _token);
                    if (task != null)
                    {
                        var result = await Connect.DeleteTaskAsync(id, _token);
                        if (result)
                            StatusMessage = "Задача успешно удалена.";
                        else
                            StatusMessage = "Не удалось удалить задачу.";

                    }
                }
                else
                {
                    StatusMessage = "Не удалось удалить задачу.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при удалении задачи: {ex.Message}";
            }
        }

        private async void CreateTask()
        {
            try
            {
                ToDoTask Task = new ToDoTask()
                {
                    Title = _Tittle,
                    Description = _description,
                    CompletedAt = _CreatedAt,
                    IsCompleted = _isCompleted,
                    UpdatedAt = null
                };
                await Connect.CreateTaskAsync(Task, _token);
                StatusMessage = "Вы успешно создали задачу";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Что-то пошло не так {ex.Data.ToString()}";
            }
        }

        private async void UpdateTask()
        {
            try
            {
              int id = 0;
                if (int.TryParse(_id, out id))
                {
                    var task = await Connect.GetTaskByIdAsync(id, _token);
                    if (task != null)
                    {
                        task.Title = _Tittle;
                        task.Description = _description;
                        task.IsCompleted = _isCompleted;

                        var result = await Connect.UpdateTaskAsync(id, task, _token);
                        if (result)
                        {
                            StatusMessage = "Задача успешно обновлена";
                        }
                        else
                        {
                            StatusMessage = "Не удалось обновить задачу";
                        }
                    }
                    else
                    {
                        StatusMessage = "Задача с таким ID не найдена";
                    }
                }
                else
                {
                    StatusMessage = "Не удалось обновить задачу";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        private async void UpdateTasks()
        {
            await LoadTasksAsync();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
