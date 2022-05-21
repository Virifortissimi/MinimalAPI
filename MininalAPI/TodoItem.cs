namespace MininalAPI
{
    public class TodoItem
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string Title { get; set; } = default!;
        public DateTime? DueAt { get; set; }
    }
}
