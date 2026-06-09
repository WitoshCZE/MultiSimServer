namespace MultiSimServer.Models;

public class UserStats
{
    public int TotalPlaySeconds { get; set; }
    public string TotalPlayTime { get; set; }

    public int SessionCount { get; set; }
    public int CommandsExecuted { get; set; }
    public int ErrorsTriggered { get; set; }
    public string LastLogin { get; set; }
    public string LastLogout { get; set; }
    public string LastActivity { get; set; }
}
