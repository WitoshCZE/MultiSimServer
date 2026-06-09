using MultiSimServer.Models;
using MultiSimServer.Services;

namespace MultiSimServer.Endpoints;

public static class LoginEndpoints
{
    public static void Map(WebApplication app, List<User> users, UserRepository repo)
    {
        app.MapPost("/login", (LoginRequest req) =>
        {
            var user = users.FirstOrDefault(u =>
                u.Username == req.Username &&
                u.Password == req.Password);

            if (user == null)
                return Results.Unauthorized();

            user.Stats.LastLogin = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            repo.SaveUsers(users);

            UserLogger.Log(user.Username, "LOGIN");

            return Results.Ok(new
            {
                success = true,
                username = user.Username,
                email = user.Email,
                workerId = user.WorkerId,
                role = user.Role,
                permission = user.Permission,
                firstName = user.FirstName,
                lastName = user.LastName,
                rank = user.Rank,
                points = user.Points,
                license = user.License,
                stats = user.Stats
            });
        });
    }
}
