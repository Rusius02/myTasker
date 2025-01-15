using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskStatus = Domain.TaskStatus;

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
    }
}
