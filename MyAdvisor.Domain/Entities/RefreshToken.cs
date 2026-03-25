namespace MyAdvisor.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public string Token { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        private RefreshToken() { Token = null!; } // For EF Core

        public RefreshToken(string token, int userId, DateTime expiryDate)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be empty.", nameof(token));

            if (expiryDate <= DateTime.UtcNow)
                throw new ArgumentException("Expiry must be in the future.", nameof(expiryDate));

            Token = token;
            UserId = userId;
            ExpiryDate = expiryDate;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiryDate;

        public void Revoke()
        {
            if (IsRevoked) return;

            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;

        }
    }
}
