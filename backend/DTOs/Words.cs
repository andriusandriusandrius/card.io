using backend.Models;

namespace backend.DTO
{
    public class NewWordRequest
    {
        public Guid FolderId;
        public string? WordText;
    }
    public class RemoveWordRequest
    {
        public Guid FolderId;
        public Guid WordId;
        
    }
}