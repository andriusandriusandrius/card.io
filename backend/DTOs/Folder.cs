namespace backend.DTO
{
    public class NewFolderRequest
    {
        public Guid UserId { set; get; }

        public string? Name { set; get; }

        public string? Language { set; get; }
    }
    public class RemoveFolderRequest
    {
        public Guid FolderId { set; get; }
        public Guid UserId { set; get; }
    }
    public class RenameFolderRequest
    {
        public Guid FolderId { set; get; }
        public Guid UserId { set; get; }
        public string? newFolderName { set; get; }
    }
}