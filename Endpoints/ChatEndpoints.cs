using Microsoft.AspNetCore.SignalR;
using MultiSimServer.Hubs;
using MultiSimServer.Models;
using MultiSimServer.Services;

namespace MultiSimServer.Endpoints;

public static class ChatEndpoints
{
    public static void Map(WebApplication app, List<User> users, ChatStorage chat)
    {
        // odeslání zprávy z klienta
        app.MapPost("/chat/send", async (ChatSendRequest req, IHubContext<ChatHub> hub) =>
        {
            if (string.IsNullOrWhiteSpace(req.Sender) ||
                string.IsNullOrWhiteSpace(req.Text))
            {
                return Results.BadRequest(new { success = false, error = "INVALID_DATA" });
            }

            var user = users.FirstOrDefault(u => u.Username == req.Sender);
            if (user == null)
                return Results.NotFound(new { success = false, error = "USER_NOT_FOUND" });

            // uložit zprávu
            var msg = chat.Add(
                req.Sender,
                req.Rank ?? user.Rank,
                string.IsNullOrWhiteSpace(req.Priority) ? "normal" : req.Priority,
                req.Text.Trim()
            );

            // 🔥 BROADCAST VŠEM KLIENTŮM PŘES SIGNALR
            await hub.Clients.All.SendAsync("ReceiveMessage", msg);

            UserLogger.Log(req.Sender, $"CHAT: {req.Text}");

            return Results.Ok(new { success = true, id = msg.Id });
        });

        // historie při přihlášení
        app.MapGet("/chat/history", (int hours) =>
        {
            if (hours <= 0 || hours > 168) // max týden
                hours = 24;

            var msgs = chat.GetHistory(hours);
            return Results.Ok(new
            {
                type = "chat_history",
                messages = msgs
            });
        });
    }
}
