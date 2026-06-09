using System.Text.Json;
using MultiSimServer.Models;

namespace MultiSimServer.Services;

public class UserRepository
{
    private readonly string _path;

    public UserRepository(string path)
    {
        _path = path;
    }

    public List<User> LoadUsers()
    {
        if (!File.Exists(_path))
            return new List<User>();

        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<User>();
    }

    public void SaveUsers(List<User> users)
    {
        File.WriteAllText(
            _path,
            JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true })
        );
    }
}
