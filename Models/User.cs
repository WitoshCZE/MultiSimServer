using MultiSimServer.Models;

namespace MultiSimServer.Models;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }

    public string Email { get; set; }
    public int WorkerId { get; set; }

    public string Role { get; set; }
    public string Permission { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Rank { get; set; }
    public int Points { get; set; }

    public LicenseInfo License { get; set; }
    public UserStats Stats { get; set; }
}
