using Microsoft.AspNetCore.SignalR;
using MultiSimServer.Hubs;

public class HeartbeatService : BackgroundService
{
    private readonly IHubContext<ChatHub> hub;

    public HeartbeatService(IHubContext<ChatHub> hub)
    {
        this.hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var dto = new
            {
                Id = -1,
                Timestamp = DateTime.Now,
                Sender = "server",
                Rank = "system",
                Priority = "system",
                Text = "OK"
            };

            await hub.Clients.All.SendAsync("ReceiveMessage", dto);

            await Task.Delay(5000, stoppingToken);
        }
    }
}
