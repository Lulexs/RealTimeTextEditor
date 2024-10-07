namespace ApplicationLogic.Exceptions;

public class AlreadyExistsException : Exception {
    public AlreadyExistsException() : base("Entity already exists") {}
    public AlreadyExistsException(string message) : base(message) {}
    public AlreadyExistsException(string message, Exception innerException) : base(message, innerException) {} 
}