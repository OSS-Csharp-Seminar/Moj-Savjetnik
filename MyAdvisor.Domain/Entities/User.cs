namespace MyAdvisor.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() // For EF Core
        {
            Username = null!;
            Email = null!;
        }

        public User(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Username = username;
            Email = email;
            CreatedAt = DateTime.UtcNow;

        }
    }
}
