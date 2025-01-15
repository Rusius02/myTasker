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
            var taskManagementControl = new TaskManagement(_taskItemService, _projectService, _memberService);
            ProjectContainer.Children.Add(projectManagementControl);
            MemberContainer.Children.Add(memberManagementControl);
            TasksContainer.Children.Add(taskManagementControl);
        }

       
    }
}
