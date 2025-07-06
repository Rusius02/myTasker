using Domain;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TaskStatus = Domain.TaskStatus;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using myTasker.Services;

namespace myTasker.Views
{
    /// <summary>
    /// Interaction logic for TaskManagement.xaml
    /// </summary>
    public partial class TaskManagement : UserControl
    {
        private readonly TaskItemService _taskItemService;

        public TaskManagement(TaskItemService taskItemService, ProjectService projectService, MemberService memberService)
        {
            InitializeComponent();
            _taskItemService = taskItemService;
            LoadProjects(projectService);
            LoadMembers(memberService);
            LoadTasks();
        }

        private async void LoadTasks()
        {
            var tasks = await _taskItemService.GetAllTasksAsync();
            TasksListView.ItemsSource = new ObservableCollection<TaskItem>(tasks);
        }

        private async void LoadProjects(ProjectService projectService)
        {
            var projects = await projectService.GetAllProjectsAsync();
            ProjectComboBox.ItemsSource = projects;
        }

        private async void LoadMembers(MemberService memberService)
        {
            var members = await memberService.GetAllMembersAsync();
            MemberComboBox.ItemsSource = members;
        }

        // Événement du bouton Ajouter
        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = ProjectComboBox.SelectedItem as Project;
            var selectedMember = MemberComboBox.SelectedItem as Member;

            var task = new TaskItem
            {
                Name = TaskNameTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                DueDate = TaskDueDatePicker.SelectedDate ?? DateTime.Now,
                Status = TaskStatus.Pending, // Statut par défaut
                ProjectId = selectedProject?.Id, 
                Project = selectedProject,
                AssignedMemberId = selectedMember?.Id,
                AssignedMember = selectedMember,
            };

            await _taskItemService.AddTaskAsync(task);
            MessageBox.Show("Tâche ajoutée !");

            LoadTasks(); // Rafraîchir la liste des tâches
        }

        private async void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is TaskItem taskItem)
            {
                // Demande de confirmation
                var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {taskItem.Name} ?",
                                             "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _taskItemService.DeleteTaskAsync(taskItem.Id);
                    LoadTasks();
                }
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
