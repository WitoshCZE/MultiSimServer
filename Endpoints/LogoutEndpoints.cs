using MultiSimServer.Models;
using MultiSimServer.Services;

namespace MultiSimServer.Endpoints;

public static class LogoutEndpoints
{
    public static void Map(WebApplication app, List<User> users, UserRepository repo)
    {
        app.MapPost("/logout", (LogoutRequest req) =>
        {
            var user = users.FirstOrDefault(u => u.Username == req.Username);
            if (user == null)
                return Results.NotFound();

            user.Stats.LastLogout = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            UserStatsService.AddTime(user.Stats, req.SessionTime);
            user.Stats.SessionCount++;

            repo.SaveUsers(users);
            UserLogger.Log(req.Username, "LOGOUT");

            return Results.Ok(new { success = true });
        });
    }
}
