using Domain;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace myTasker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TaskModel> TaskList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            TaskList = new ObservableCollection<TaskModel>();

            // Lier la collection au ListView
            TaskListView.ItemsSource = TaskList;
        }
        // Événement du bouton Ajouter
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier que le champ de texte n'est pas vide
            if (!string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                // Ajouter une nouvelle tâche à la collection
                TaskList.Add(new TaskModel { Name = TaskInput.Text });
                // Réinitialiser le champ de texte
                TaskInput.Clear();
            }
            else
            {
                MessageBox.Show("Entrez une tâche valide !");
            }
        }
    }
}
