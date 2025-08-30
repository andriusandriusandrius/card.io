using backend.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
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
        if (!result.success)
            return BadRequest(new { result.message });

        return Ok(new { result.message, result.word });
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

        if (!result.success)
            return BadRequest(new {result.message });

        return Ok(new {  result.message });
    }


}