using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Common.Data
{
    public class Permission
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}