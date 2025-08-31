using backend.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);


    }
    [HttpDelete("{folderId}")]
    public async Task<IActionResult> RemoveFolder( Guid folderId)
    {
         var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
         if (userIdClaim == null)
            return Unauthorized("Invalid token");

         Guid userId = Guid.Parse(userIdClaim.Value);
        RemoveFolderRequest removeFolderRequest = new()
        {
            FolderId = folderId,
            UserId = userId
        };

        var result = await _folderService.Remove(removeFolderRequest);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpPut("{folderId}")]
    public async Task<IActionResult> RenameFolder(Guid folderId, [FromBody] RenameFolderRequest renameFolderRequest)
    {
        renameFolderRequest.FolderId = folderId;
        var result = await _folderService.Rename(renameFolderRequest);
         if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
}