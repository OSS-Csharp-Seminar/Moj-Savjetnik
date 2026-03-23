namespace MyAdvisor.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User()
        {
            Username = null!;
            Email = null!;
            PasswordHash = null!;
        }

        public User(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Username = username;
            Email = email;
            PasswordHash = string.Empty;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));

            PasswordHash = passwordHash;
        }
    }
}
