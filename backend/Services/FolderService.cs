using backend.Data;
using backend.DTO;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

public interface IFolderService
{
    Task<ApiResponse<Folder>> Add(NewFolderRequest newFolderRequest);
    Task<ApiResponse<Folder>> Remove(RemoveFolderRequest removeFolderRequest);
    Task<ApiResponse<Folder>> Rename(RenameFolderRequest renameFolderRequest);
}
public class FolderService : IFolderService
{
    private readonly CardioContext _db;

    public FolderService(CardioContext db) {
        _db = db;
    }
    public async Task<ApiResponse<Folder>> Add(NewFolderRequest newFolderRequest)
    {
        try
        {
            User user = await _db.Users.Include(u => u.Folders).FirstOrDefaultAsync(u => u.Id == newFolderRequest.UserId);

            if (user == null)
                return new ApiResponse<Folder> {Success=false, Message="No such user", Data=null };

            Folder folder = new()
            {
                Name = newFolderRequest.Name,
                Language = newFolderRequest.Language,
                User = user,
                UserId = user.Id,
            };
            _db.Folders.Add(folder);
            await _db.SaveChangesAsync();
            return new ApiResponse<Folder> { Success = true, Message = $"Success! Added a new folder by the name of {folder.Name} to the user - {user.Email}", Data = folder };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Folder> {Success=false, Message=$"Failed to add a new folder: {ex.Message}",Data=null };
            
        }
    }

    public async Task<ApiResponse<Folder>> Remove(RemoveFolderRequest removeFolderRequest)
    {
        try
        {
            Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == removeFolderRequest.FolderId && f.UserId == removeFolderRequest.UserId);
            if (folder == null)
                return new ApiResponse<Folder> { Success=false,Message="This user has no such folder",Data=null };

            _db.Folders.Remove(folder);
            await _db.SaveChangesAsync();

            return new ApiResponse<Folder> { Success=true, Message=$"Success! The folder by the name of {folder.Name} has been removed",Data=null };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Folder> {Success=false, Message=$"Failed to remove folder: {ex.Message}",Data=null };
        }
    }
    public async Task<ApiResponse<Folder>> Rename(RenameFolderRequest renameFolderRequest)
    {
        try
        {
            Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == renameFolderRequest.FolderId && f.UserId == renameFolderRequest.UserId);
            if (folder == null)
                return new ApiResponse<Folder> { Success = false, Message = "This user has no such folder", Data = null };
            folder.Name = renameFolderRequest.NewFolderName;
            await _db.SaveChangesAsync();
            return new ApiResponse<Folder>{Success=true, Message=$"Success! Folder has been renamed to {folder.Name}", Data=folder};
        }
        catch (Exception ex)
        {
            return new ApiResponse<Folder> {Success=false, Message=$"Failed to rename folder {ex.Message}", Data=null };
        }
    }
}