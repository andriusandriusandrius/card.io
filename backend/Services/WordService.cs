using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

using LibreTranslate.Client.Net;
using LibreTranslate.Client.Net.Models;
using backend.DTO;
public interface IWordService
{
    Task<(bool success, string message, Word? word)> Add(NewWordRequest newWordRequest);
    Task<string> Translate(Folder folder, string wordText);
    Task<(bool success, string message)> Remove(RemoveWordRequest removeWordRequest);
}
public class WordService : IWordService
{
    private readonly CardioContext _db;

    public WordService(CardioContext db)
    {
        _db = db;
    }
    public async Task<(bool success, string message, Word? word)> Add(NewWordRequest newWordRequest)
    {
        try
        {
            Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == newWordRequest.FolderId);
            if (folder == null)
                return (false, "No such folder", null);
            Word word = new()
            {
                WordText = newWordRequest.WordText,
                FolderId = newWordRequest.FolderId,
                Folder = folder,
                TranslatedWordText = await Translate(folder, newWordRequest.WordText)
            };
            _db.Words.Add(word);
            await _db.SaveChangesAsync();

            return (true, $"Success! Added word {word.WordText}", word);
        }
        catch (Exception ex)
        {
            return (false, $"Failed to add word: {ex.Message}", null);
        }
    }
    public async Task<string> Translate(Folder folder, string wordText)
    {
        try
        {
            var options = new LibreTranslateClientOptions("https://libretranslate.com", null);
            using var client = new LibreTranslateClient(options, null);

            var result = await client.TranslateAsync(wordText, folder.Language, "en");

            return result.Value?.TranslatedText ?? wordText;
        }
        catch
        {
            return wordText;
        }
    }
    public async Task<(bool success, string message)> Remove(RemoveWordRequest removeWordRequest)
    {
        try
        {
            Word word = await _db.Words.FirstOrDefaultAsync(w => w.FolderId == removeWordRequest.FolderId && w.Id == removeWordRequest.WordId);
            if (word == null)
                return (false, "There is no such word in the specified folder");

            _db.Words.Remove(word);
            await _db.SaveChangesAsync();
            return (true, $"Success! Word removed!");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to remove word: {ex.Message}");
        }
    }
}