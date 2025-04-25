using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Common.Data
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [MaxLength(255)]
        public string PasswordHash { get; set; } = null!;

        [MaxLength(255)]
        public string? FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}