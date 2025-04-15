namespace _6MD.AuthServer.utils.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found")
        {
        }
        public UserNotFoundException(string username) : base($"User {username} not found")
        {
        }
    }
    public class PasswordNotSetException : Exception
    {
        public PasswordNotSetException() : base("Password not set")
        {
        }
        public PasswordNotSetException(string username) : base($"Password not set for user {username}")
        {
        }
    }
}
