using System.Text.Json;
using MultiSimServer.Models;

namespace MultiSimServer.Services;

public class ChatStorage
{
    private readonly string _path;
    private List<ChatMessageDto> _messages = new();
    private int _lastId = 0;

    public ChatStorage(string path)
    {
        _path = path;

        if (File.Exists(_path))
        {
            var json = File.ReadAllText(_path);
            var loaded = JsonSerializer.Deserialize<List<ChatMessageDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (loaded != null)
            {
                _messages = loaded;
                if (_messages.Count > 0)
                    _lastId = _messages.Max(m => m.Id);
            }
        }
    }

    public IReadOnlyList<ChatMessageDto> Messages => _messages;

    public ChatMessageDto Add(string sender, string rank, string priority, string text)
    {
        var msg = new ChatMessageDto
        {
            Id = ++_lastId,
            Timestamp = DateTime.UtcNow,
            Sender = sender,
            Rank = rank,
            Priority = string.IsNullOrWhiteSpace(priority) ? "normal" : priority,
            Text = text
        };

        _messages.Add(msg);
        Save();
        Cleanup();

        return msg;
    }

    public IEnumerable<ChatMessageDto> GetHistory(int hours)
    {
        var cutoff = DateTime.UtcNow.AddHours(-hours);

        return _messages
            .Where(m =>
                m.Priority == "system" ||
                m.Priority == "high" ||
                m.Timestamp >= cutoff)
            .OrderBy(m => m.Timestamp)
            .ToList();
    }

    private void Save()
    {
        File.WriteAllText(
            _path,
            JsonSerializer.Serialize(_messages, new JsonSerializerOptions { WriteIndented = true })
        );
    }

    private void Cleanup()
    {
        var now = DateTime.UtcNow;

        _messages.RemoveAll(m =>
            (m.Priority == "normal" && m.Timestamp < now.AddHours(-24)) ||
            (m.Priority == "high" && m.Timestamp < now.AddHours(-12)) ||
            (m.Priority == "system" && m.Timestamp < now.AddHours(-48))
        );
    }
}
