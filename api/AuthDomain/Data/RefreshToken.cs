using System;
using System.ComponentModel.DataAnnotations;

namespace AuthDomain.Data
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [MaxLength(255)]
        public string Token { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? CreatedByIp { get; set; }

        public User User { get; set; } = null!;
    }
}