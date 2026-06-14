using MultiSimServer.Models;
using MultiSimServer.Services;

namespace MultiSimServer.Endpoints;

public static class ProfileEndpoints
{
    public static void Map(WebApplication app, List<User> users, UserRepository repo)
    {
        app.MapPost("/updateProfile", (UpdateProfileRequest req) =>
        {
            if (string.IsNullOrWhiteSpace(req.Username) ||
                string.IsNullOrWhiteSpace(req.Email))
            {
                return Results.BadRequest(new { success = false, error = "INVALID_DATA" });
            }

            var user = users.FirstOrDefault(u => u.Username == req.Username);
            if (user == null)
                return Results.NotFound(new { success = false, error = "USER_NOT_FOUND" });

            // jediná věc, kterou může měnit sám uživatel
            user.Email = req.Email;

            repo.SaveUsers(users);
            UserLogger.Log(req.Username, "PROFILE_UPDATED");

            return Results.Ok(new { success = true });
        });
    }
}
