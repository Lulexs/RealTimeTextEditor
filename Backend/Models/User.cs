namespace Models;

public class User {
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class UserByUsername : User {
    public UserByUsername() {}

    public UserByUsername(User user) {
        UserId = user.UserId;
        Username = user.Username;
        Password = user.Password;
    }
}