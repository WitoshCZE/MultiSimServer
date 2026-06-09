namespace MultiSimServer.Models;

public record ChangePasswordRequest(string Username, string OldPassword, string NewPassword);
