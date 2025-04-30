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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsBlocked { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; } = [];
        public ICollection<UserPermission> UserPermissions { get; set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
        public ICollection<Vocabulary> Vocabularies { get; set; } = [];
    }
}