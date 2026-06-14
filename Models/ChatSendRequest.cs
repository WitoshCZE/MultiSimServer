namespace MultiSimServer.Models;

public class ChatSendRequest
{
    public string Sender { get; set; }
    public string Rank { get; set; }
    public string Priority { get; set; } = "normal";
    public string Text { get; set; }
}
