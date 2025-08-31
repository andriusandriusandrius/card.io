using backend.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WordController : ControllerBase
{
    private readonly IWordService _wordService;
    public WordController(IWordService wordService)
    {
        _wordService = wordService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] NewWordRequest newWordRequest)
    {
        var result = await _wordService.Add(newWordRequest);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpDelete("{folderId}/{wordId}")]
    public async Task<IActionResult> Remove(Guid folderId, Guid wordId)
    { 
        RemoveWordRequest removeWordRequest = new()
        {
            FolderId = folderId,
            WordId = wordId
        };

        var result = await _wordService.Remove(removeWordRequest);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    

}