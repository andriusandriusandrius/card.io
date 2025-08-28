namespace backend.Models
{
    public class User()
    {
        public Guid Id { set; get; } = Guid.NewGuid();
        public string? Email { set; get; }
        public string? HashedPassword { set; get; }
        public List<Folder> Folders { set; get; } = new();
    }
}