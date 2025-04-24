using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthDomain.Data
{
    public class Group
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = [];
        public ICollection<GroupPermission> GroupPermissions { get; set; } = [];
    }
}