namespace backend.Models
{
    public class Folder()
    {
        public Guid Id { set; get; } = Guid.NewGuid();
        public string? Name { set; get; }
        public string? Language { set; get; }

        public Guid UserId { set; get; }

        public User? User { set; get; }
        public List<Word> Words { set; get; } = new();
    }
}