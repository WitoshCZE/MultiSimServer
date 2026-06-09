namespace MultiSimServer.Services;

public static class UserLogger
{
    private static readonly string LogDir = "logs";

    static UserLogger()
    {
        if (!Directory.Exists(LogDir))
            Directory.CreateDirectory(LogDir);
    }

    public static void Log(string username, string message)
    {
        string file = Path.Combine(LogDir, $"{username}.log");
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        File.AppendAllText(file, line + Environment.NewLine);
    }
}
