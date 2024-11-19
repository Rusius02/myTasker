using ClosedXML.Excel;
using Domain;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

            string filePath = "D:\\GitProjects\\WPF\\Tasker\\src\\MesTâches.xlsx";
            ReadFromExcel(filePath, TaskList);

            // Lier la collection au ListView
            TaskListView.ItemsSource = TaskList;
        }
        public void ExportToExcel (ObservableCollection<TaskModel> tasks, string filePath)
        {
            using ( var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Taches");

                worksheet.Cell(1, 1).Value = "Mes tâches";
                worksheet.Cell(1, 1).Style.Font.Bold=true;
                for (int i = 0; i < tasks.Count; i++)
                {
                    worksheet.Cell(i+2, 1).Value = tasks[i].Name;
                }
                workbook.SaveAs(filePath);
            }
        }
        private void ReadFromExcel(string filePath, ObservableCollection<TaskModel> taskList)
        {
            // Vérifier que le fichier existe
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Le fichier Excel n'existe pas !");
                return;
            }

            try
            {
                // Charger le fichier Excel
                using (var workbook = new XLWorkbook(filePath))
                {
                    // Accéder à la première feuille (ou utiliser le nom si connu)
                    var worksheet = workbook.Worksheets.First();

                    // Lire les lignes, en commençant par la deuxième ligne (ignorer l'en-tête)
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        // Lire la valeur de la première colonne
                        string taskName = row.Cell(1).GetString();

                        // Ajouter la tâche à la liste
                        if (!string.IsNullOrWhiteSpace(taskName))
                        {
                            taskList.Add(new TaskModel { Name = taskName });
                        }
                    }
                }

                MessageBox.Show("Les tâches ont été chargées avec succès !");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la lecture du fichier Excel : {ex.Message}");
            }
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
