using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

public interface IFolderService
{
    Task<(bool success, string message, Folder? folder)> Add(Guid userId, string folderName, string folderLanguage);
    Task<(bool success, string message)> Remove(Guid folderId, Guid userId);
    Task<(bool success, string message, Folder? folder)> Rename(Guid folderId, Guid userId, string newFolderName);
}
public class FolderService : IFolderService
{
    private readonly CardioContext _db;

    public FolderService(CardioContext db) {
        _db = db;
    }
    public async Task<(bool success, string message, Folder? folder)> Add(Guid userId, string folderName, string folderLanguage)
    {
        User user = await _db.Users.Include(u => u.Folders).FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return (false, "No such user", null);

        Folder folder = new()
        {
            Name = folderName,
            Language = folderLanguage,
            User = user,
            UserId = user.Id,
        };
        _db.Folders.Add(folder);
        await _db.SaveChangesAsync();
        return (true, $"Success! Added a new folder by the name of {folder.Name} to the user - {user.Email}", folder);
    }

    public async Task<(bool success, string message)> Remove(Guid folderId, Guid userId)
    {
        Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == folderId && f.UserId == userId);
        if (folder == null)
            return (false, "This user has no such folder");

        _db.Folders.Remove(folder);
        await _db.SaveChangesAsync();

        return (true, $"Success! The folder by the name of {folder.Name} has been removed");
    }
    public async Task<(bool success, string message, Folder? folder)> Rename(Guid folderId, Guid userId, string newFolderName)
    {
        Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == folderId && f.UserId == userId);
        if (folder == null)
            return (false, "This user has no such folder", null);
        folder.Name = newFolderName;
        await _db.SaveChangesAsync();
        return (true, $"Success! Folder has been renamed to {folder.Name}",folder);
    }
}