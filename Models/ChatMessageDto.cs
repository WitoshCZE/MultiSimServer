namespace MultiSimServer.Models;

public class ChatMessageDto
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }

    public string Sender { get; set; }
    public string Rank { get; set; }
    public string Priority { get; set; } // normal / high / system
    public string Text { get; set; }
}
