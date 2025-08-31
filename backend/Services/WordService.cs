using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

using LibreTranslate.Client.Net;
using backend.DTO;
using backend.DTOs;
public interface IWordService
{
    Task<ApiResponse<Word>> Add(NewWordRequest newWordRequest);
    Task<string> Translate(Folder folder, string wordText);
    Task<ApiResponse<Word>> Remove(RemoveWordRequest removeWordRequest);
}
public class WordService : IWordService
{
    private readonly CardioContext _db;

    public WordService(CardioContext db)
    {
        _db = db;
    }
    public async Task<ApiResponse<Word>> Add(NewWordRequest newWordRequest)
    {
        try
        {
            Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == newWordRequest.FolderId);
            if (folder == null)
                return new ApiResponse<Word> { Success = false, Message = "No such folder", Data = null };
            Word word = new()
            {
                WordText = newWordRequest.WordText,
                FolderId = newWordRequest.FolderId,
                Folder = folder,
                TranslatedWordText = await Translate(folder, newWordRequest.WordText)
            };
            _db.Words.Add(word);
            await _db.SaveChangesAsync();

            return new ApiResponse<Word> { Success = true, Message = $"Success! Added word {word.WordText}", Data = word };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Word>{Success=false, Message =$"Failed to add word: {ex.Message}", Data = null};
        }
    }
    public async Task<string> Translate(Folder folder, string wordText)
    {
        try
        {
            var options = new LibreTranslateClientOptions("http://localhost:5000", null);
            using var client = new LibreTranslateClient(options, null);

            var result = await client.TranslateAsync(wordText, folder.Language, "en");

            return result.Value?.TranslatedText ?? wordText;
        }
        catch
        {
            return wordText;
        }
    }
    public async Task<ApiResponse<Word>> Remove(RemoveWordRequest removeWordRequest)
    {
        try
        {
            Word word = await _db.Words.FirstOrDefaultAsync(w => w.FolderId == removeWordRequest.FolderId && w.Id == removeWordRequest.WordId);
            if (word == null)
                return new ApiResponse<Word> { Success = false, Message = "There is no such word in the specified folder", Data = null };

            _db.Words.Remove(word);
            await _db.SaveChangesAsync();
            return new ApiResponse<Word> { Success = true, Message = $"Success! Word removed!", Data = null };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Word>{Success = false, Message =$"Failed to remove word: {ex.Message}", Data = null};
        }
    }
}