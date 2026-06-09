using MultiSimServer.Models;

namespace MultiSimServer.Services;

public static class UserStatsService
{
    public static void AddTime(UserStats stats, int seconds)
    {
        stats.TotalPlaySeconds += seconds;

        var ts = TimeSpan.FromSeconds(stats.TotalPlaySeconds);
        stats.TotalPlayTime = $"{(int)ts.TotalHours}h {ts.Minutes}m";
    }
}
