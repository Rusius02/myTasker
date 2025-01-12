using ClosedXML.Excel;
using Domain;
using myTasker.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskStatus = Domain.TaskStatus;

namespace myTasker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TaskItem> TaskList { get; set; }
        private readonly ProjectService _projectService;
        private readonly TaskItemService _taskItemService;
        private readonly MemberService _memberService;
        public MainWindow(ProjectService projectService, TaskItemService taskItemService, MemberService memberService)
        {
            InitializeComponent();
            _projectService = projectService;
            _taskItemService = taskItemService;
            _memberService = memberService;
            var projectManagementControl = new ProjectManagement(_projectService);
            var memberManagementControl = new MemberManagement(_memberService);
            ProjectContainer.Children.Add(projectManagementControl);
            MemberContainer.Children.Add(memberManagementControl);
            TaskList = new ObservableCollection<TaskItem>();
            LoadTasks();
            _memberService = memberService;
        }

        private async void LoadTasks()
        {
            var tasks = await _taskItemService.GetAllTasksAsync();
            TaskDataGrid.ItemsSource = new ObservableCollection<TaskItem>(tasks);
        }

      
        // Événement du bouton Ajouter
        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new TaskItem
            {
                Name = TaskNameTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                DueDate = TaskDueDatePicker.SelectedDate ?? DateTime.Now,
                Status = TaskStatus.Pending // Statut par défaut
            };

            await _taskItemService.AddTaskAsync(task);
            MessageBox.Show("Tâche ajoutée !");
            LoadTasks(); // Rafraîchir la liste
        }
    }
}
