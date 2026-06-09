using MultiSimServer.Models;
using MultiSimServer.Services;

namespace MultiSimServer.Endpoints;

public static class ActivityEndpoints
{
    public static void Map(WebApplication app, List<User> users, UserRepository repo)
    {
        app.MapPost("/activity", (ActivityRequest req) =>
        {
            var user = users.FirstOrDefault(u => u.Username == req.Username);
            if (user == null)
                return Results.NotFound();

            user.Stats.LastActivity = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            repo.SaveUsers(users);
            UserLogger.Log(req.Username, "ACTIVITY");

            return Results.Ok(new { success = true });
        });
    }
}
