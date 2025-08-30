using backend.Models;

namespace backend.DTO
{
    public class NewWordRequest
    {
        public Guid FolderId { set; get; }
        public string? WordText { set; get; }
    }
    public class RemoveWordRequest
    {
        public Guid FolderId { set; get; }
        public Guid WordId { set; get; }
        
    }
}