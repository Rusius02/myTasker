namespace Domain
{
    public class TaskModel
    {
        public string Name { get; set; }
        public string Status { get; set; } = "À faire";
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime DoneAt { get; set; }
    }
}