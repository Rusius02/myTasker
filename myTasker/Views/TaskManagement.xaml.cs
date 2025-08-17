using Domain;
using myTasker.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace myTasker.Views
{
    public partial class TaskManagement : UserControl
    {
        private readonly TaskItemService _taskItemService;
        private readonly ProjectService _projectService;
        private readonly MemberService _memberService;
        private ObservableCollection<TaskItem> _tasks;
        private ICollectionView _tasksView;

        public TaskManagement(TaskItemService taskItemService, ProjectService projectService, MemberService memberService)
        {
            InitializeComponent();

            _taskItemService = taskItemService;
            _projectService = projectService;
            _memberService = memberService;
            LoadProjects();
            LoadMembers();
            LoadTasks();
        }

        private async void LoadTasks()
        {
            var tasks = await _taskItemService.GetAllTasksAsync();
            _tasks = new ObservableCollection<TaskItem>(tasks);
            _tasksView = CollectionViewSource.GetDefaultView(_tasks);
            TasksListView.ItemsSource = _tasksView;
        }
        private async void LoadProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            ProjectFilterComboBox.ItemsSource = projects;
            ProjectComboBox.ItemsSource = projects;
        }

        private async void LoadMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            MemberFilterComboBox.ItemsSource = members;
            MemberComboBox.ItemsSource = members;
        }
        private void ApplyFilters()
        {
            _tasksView.Filter = obj =>
            {
                if (obj is TaskItem task)
                {
                    bool projectMatch = ProjectFilterComboBox.SelectedItem == null || task.ProjectId == ((Project)ProjectFilterComboBox.SelectedItem).Id;
                    bool memberMatch = MemberFilterComboBox.SelectedItem == null || task.AssignedMemberId == ((Member)MemberFilterComboBox.SelectedItem).Id;
                    return projectMatch && memberMatch;
                }
                return false;
            };
            _tasksView.Refresh();
        }
        private void ProjectFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void MemberFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = ProjectComboBox.SelectedItem as Project;
            var selectedMember = MemberComboBox.SelectedItem as Member;

            var selectedStatus = Domain.TaskStatus.Pending;

            var task = new TaskItem
            {
                Name = TaskNameTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                DueDate = TaskDueDatePicker.SelectedDate ?? DateTime.Now,
                Status = selectedStatus,
                ProjectId = selectedProject?.Id,
                Project = selectedProject,
                AssignedMemberId = selectedMember?.Id,
                AssignedMember = selectedMember,
            };

            await _taskItemService.AddTaskAsync(task);
            MessageBox.Show("Tâche ajoutée !");
            LoadTasks();
        }

        private async void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is TaskItem taskItem)
            {
                var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {taskItem.Name} ?",
                                             "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _taskItemService.DeleteTaskAsync(taskItem.Id);
                    LoadTasks();
                }
            }
        }
        private async void TaskStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cb && cb.DataContext is TaskItem task)
            {
                await _taskItemService.UpdateTaskAsync(task);
            }
        }
        private async void ExportTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var tasks = await _taskItemService.GetAllTasksAsync();
            if (tasks == null || !tasks.Any())
            {
                MessageBox.Show("Aucune tâche à exporter.");
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "taches_export",
                Filter = "Fichier Excel (*.xlsx)|*.xlsx|Fichier CSV (*.csv)|*.csv",
                DefaultExt = ".xlsx"
            };

            if (saveDialog.ShowDialog() == true)
            {
                ExportFormat format;
                string extension = System.IO.Path.GetExtension(saveDialog.FileName).ToLower();

                switch (extension)
                {
                    case ".xlsx":
                        format = ExportFormat.Xlsx;
                        break;
                    case ".csv":
                        format = ExportFormat.Csv;
                        break;
                    default:
                        MessageBox.Show("Format non supporté.");
                        return;
                }

                try
                {
                    var exporter = new ExportService();
                    await exporter.ExportAsync(tasks, format, saveDialog.FileName);
                    MessageBox.Show("Export terminé !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'export : {ex.Message}");
                }
            }
        }
    }
}
