using MultiSimServer.Endpoints;
using MultiSimServer.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var repo = new UserRepository("users.json");
var users = repo.LoadUsers();

LoginEndpoints.Map(app, users, repo);
LogoutEndpoints.Map(app, users, repo);
ActivityEndpoints.Map(app, users, repo);
PasswordEndpoints.Map(app, users, repo);

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
