namespace Models;

public class UserWorkspace {
    public required Guid UserId { get; set; }
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public bool IsOwner { get; set; }
}