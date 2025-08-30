using backend.Data;
using backend.DTO;
using backend.Models;
using Microsoft.EntityFrameworkCore;

public interface IFolderService
{
    Task<(bool success, string message, Folder? folder)> Add(NewFolderRequest newFolderRequest);
    Task<(bool success, string message)> Remove(RemoveFolderRequest removeFolderRequest);
    Task<(bool success, string message, Folder? folder)> Rename(RenameFolderRequest renameFolderRequest);
}
public class FolderService : IFolderService
{
    private readonly CardioContext _db;

    public FolderService(CardioContext db) {
        _db = db;
    }
    public async Task<(bool success, string message, Folder? folder)> Add(NewFolderRequest newFolderRequest)
    {
        User user = await _db.Users.Include(u => u.Folders).FirstOrDefaultAsync(u => u.Id == newFolderRequest.UserId);

        if (user == null)
            return (false, "No such user", null);

        Folder folder = new()
        {
            Name = newFolderRequest.Name,
            Language = newFolderRequest.Language,
            User = user,
            UserId = user.Id,
        };
        _db.Folders.Add(folder);
        await _db.SaveChangesAsync();
        return (true, $"Success! Added a new folder by the name of {folder.Name} to the user - {user.Email}", folder);
    }

    public async Task<(bool success, string message)> Remove(RemoveFolderRequest removeFolderRequest)
    {
        Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == removeFolderRequest.FolderId && f.UserId == removeFolderRequest.UserId);
        if (folder == null)
            return (false, "This user has no such folder");

        _db.Folders.Remove(folder);
        await _db.SaveChangesAsync();

        return (true, $"Success! The folder by the name of {folder.Name} has been removed");
    }
    public async Task<(bool success, string message, Folder? folder)> Rename(RenameFolderRequest renameFolderRequest)
    {
        Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == renameFolderRequest.FolderId && f.UserId == renameFolderRequest.UserId);
        if (folder == null)
            return (false, "This user has no such folder", null);
        folder.Name = renameFolderRequest.newFolderName;
        await _db.SaveChangesAsync();
        return (true, $"Success! Folder has been renamed to {folder.Name}",folder);
    }
}