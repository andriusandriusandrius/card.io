using System.Linq.Expressions;
using backend.DTO;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class FolderController : ControllerBase
{
    private readonly IFolderService _folderService;
    public FolderController(IFolderService folderService)
    {
        _folderService = folderService;
    }
    [HttpPost]
    public async Task<IActionResult> AddFolder([FromBody] NewFolderRequest newFolderRequest)
    {
        var result = await _folderService.Add(newFolderRequest);
        if (!result.success)
            return BadRequest(new { message = result.message });

        return Ok(new { message = result.message, folder = result.folder });


    }
    [HttpDelete("{userId}/{folderId}")] // will change the link (remove userId) here once JWT token authenthication will be done
    public async Task<IActionResult> RemoveFolder(Guid userId, Guid folderId)
    {
        RemoveFolderRequest removeFolderRequest = new()
        {
            FolderId = folderId,
            UserId = userId
        };

        var result = await _folderService.Remove(removeFolderRequest);

        if (!result.success)
            return BadRequest(new { message = result.message });

        return Ok(new { message = result.message });
    }
    [HttpPut("{folderId}")]
    public async Task<IActionResult> RenameFolder(Guid folderId, [FromBody] RenameFolderRequest renameFolderRequest)
    {
        renameFolderRequest.FolderId = folderId;
        var result = await _folderService.Rename(renameFolderRequest);
         if (!result.success)
            return BadRequest(new { message = result.message });

        return Ok(new { message = result.message });
    }
    
}