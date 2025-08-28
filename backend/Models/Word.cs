namespace backend.Models
{
    public class Word()
    {
        public Guid Id { set; get; }

        public string? WordText { set; get; }

        public Guid FolderId { set; get; }

        public Folder? Folder { set; get; }
        public string? TranslatedWordText { set; get; }
        
    }
}