namespace Models;

public class User {
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
