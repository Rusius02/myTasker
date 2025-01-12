using Domain;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace myTasker.Views
{
    /// <summary>
    /// Interaction logic for ProjectManagement.xaml
    /// </summary>
    public partial class ProjectManagement : UserControl
    {
        private readonly ProjectService _projectService;

        public ProjectManagement(ProjectService projectService)
        {
            InitializeComponent();
            _projectService = projectService;
            LoadProjects();
        }

        private async void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var project = new Project
            {
                Name = ProjectNameTextBox.Text,
                Description = ProjectDescriptionTextBox.Text,
                StartDate = StartDatePicker.SelectedDate ?? DateTime.Now,
                EndDate = EndDatePicker.SelectedDate ?? DateTime.Now.AddMonths(1)
            };

            await _projectService.AddProjectAsync(project);
            MessageBox.Show("Project added successfully!");

            // Clear input fields
            ProjectNameTextBox.Text = string.Empty;
            ProjectDescriptionTextBox.Text = string.Empty;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            // Refresh the project list
            LoadProjects();
        }

        private async void LoadProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            ProjectsListView.ItemsSource = projects;
        }
    }
}
