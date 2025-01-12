namespace Domain
{
    public class TaskItem
    {
        public int Id { get; set; } // Identifiant unique
        public string Name { get; set; } // Nom de la tâche
        public string Description { get; set; } // Description de la tâche
        public DateTime DueDate { get; set; } // Date d'échéance
        public TaskPriority Priority { get; set; } // Priorité de la tâche
        public TaskStatus Status { get; set; } // Statut de la tâche
        public int? AssignedMemberId { get; set; } // Membre assigné (nullable)
        public Member AssignedMember { get; set; } // Référence au membre assigné

        public TaskItem()
        {
        }
    }
}