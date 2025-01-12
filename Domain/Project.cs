using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Project
    {
        public int Id { get; set; } // Identifiant unique
        public string Name { get; set; } // Nom du projet
        public string Description { get; set; } // Description du projet
        public DateTime StartDate { get; set; } // Date de début
        public DateTime EndDate { get; set; } // Date de fin prévue
        public ProjectStatus Status { get; set; } // Statut du projet
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>(); // Liste des tâches associées

        public Project()
        {
        }
    }
}
