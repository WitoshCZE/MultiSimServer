using MultiSimServer.Endpoints;
using MultiSimServer.Services;
using MultiSimServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// SignalR
builder.Services.AddSignalR();

// 🔥 REGISTRACE HEARTBEAT SLUŽBY
builder.Services.AddHostedService<HeartbeatService>();

var app = builder.Build();

// ---------------------------------------------------------
// 🔥 INTERAKTIVNÍ VÝBĚR REŽIMU SERVERU
// ---------------------------------------------------------

string mode = "local";

Console.WriteLine("Zadej režim serveru (local / lan).");
Console.WriteLine("Pokud nic nezadáš, za 10 sekund se spustí LOCAL.");

var inputTask = Task.Run(() => Console.ReadLine());
if (await Task.WhenAny(inputTask, Task.Delay(10000)) == inputTask)
{
    var typed = inputTask.Result?.Trim().ToLower();
    if (typed == "lan" || typed == "local")
        mode = typed;
}
else
{
    Console.WriteLine("⏳ Nebyla zadána žádná volba → spouštím LOCAL režim.");
}

// ---------------------------------------------------------
// 🔥 NASTAVENÍ URL PODLE REŽIMU
// ---------------------------------------------------------

if (mode == "lan")
{
    app.Urls.Add("http://0.0.0.0:5188");
    Console.WriteLine("🌐 Server běží v LAN režimu (0.0.0.0:5188)");

    // výpis LAN IP adres
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
            Console.WriteLine($"➡ LAN adresa: http://{ip}:5188");
    }
}
else
{
    app.Urls.Add("http://localhost:5188");
    Console.WriteLine("💻 Server běží v LOCAL režimu (localhost:5188)");
}

// ---------------------------------------------------------

var repo = new UserRepository("users.json");
var users = repo.LoadUsers();

var chatStorage = new ChatStorage("chat_history.json");

LoginEndpoints.Map(app, users, repo);
LogoutEndpoints.Map(app, users, repo);
ActivityEndpoints.Map(app, users, repo);
PasswordEndpoints.Map(app, users, repo);
ProfileEndpoints.Map(app, users, repo);

ChatEndpoints.Map(app, users, chatStorage);

app.MapHub<ChatHub>("/chatHub");

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
