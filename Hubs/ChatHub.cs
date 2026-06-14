using Microsoft.AspNetCore.SignalR;

namespace MultiSimServer.Hubs
{
    public class ChatHub : Hub
    {
        // Volitelné: log připojení
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"[ChatHub] Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        // Volitelné: log odpojení
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"[ChatHub] Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
