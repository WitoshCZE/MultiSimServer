using MultiSimServer.Models;
using MultiSimServer.Services;

namespace MultiSimServer.Endpoints;

public static class PasswordEndpoints
{
    public static void Map(WebApplication app, List<User> users, UserRepository repo)
    {
        app.MapPost("/changePassword", (ChangePasswordRequest req) =>
        {
            var user = users.FirstOrDefault(u => u.Username == req.Username);

            if (user == null)
                return Results.NotFound();

            if (user.Password != req.OldPassword)
                return Results.Unauthorized();

            user.Password = req.NewPassword;

            repo.SaveUsers(users);
            UserLogger.Log(req.Username, "PASSWORD_CHANGED");

            return Results.Ok(new { success = true });
        });
    }
}
