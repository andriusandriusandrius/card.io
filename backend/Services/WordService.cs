using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

using LibreTranslate.Client.Net;
using LibreTranslate.Client.Net.Models;
public interface IWordService
{
    Task<(bool success, string message, Word? word)> Add(Guid folderId, string wordText);
    Task<string> Translate(Folder folder, Guid wordId);
    Task<(bool success, string message)> Remove(Guid folderId, Guid wordId);
}
public class WordService
{
    private readonly CardioContext _db;

    public WordService(CardioContext db)
    {
        _db = db;
    }
    public async Task<(bool success, string message, Word? word)> Add(Guid folderId, string wordText)
    {
        try
        {
            Folder folder = await _db.Folders.FirstOrDefaultAsync(f => f.Id == folderId);
            if (folder == null)
                return (false, "No such folder", null);
            Word word = new()
            {
                WordText = wordText,
                FolderId = folderId,
                Folder = folder,
                TranslatedWordText = await Translate(folder, wordText)
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

            return result.ToString() ?? wordText;
        }
        catch
        {
            return wordText;
        }
    }
    public async Task<(bool success, string message)> Remove(Guid folderId, Guid wordId)
    {
        try
        {
            Word word = await _db.Words.FirstOrDefaultAsync(w => w.FolderId == folderId && w.Id == wordId);
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