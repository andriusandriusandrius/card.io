using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

public class FolderController : ControllerBase
{
    private readonly IFolderService _folderService;
    public FolderController(IFolderService folderService)
    {
        _folderService = folderService;
    }
    
    
}