using api.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Common
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<GroupPermission> GroupPermissions { get; set; } = null!;
        public DbSet<UserGroup> UserGroups { get; set; } = null!;
        public DbSet<UserPermission> UserPermissions { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupPermission>().HasKey(gp => new { gp.GroupId, gp.PermissionId });
            modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });
            modelBuilder.Entity<UserPermission>().HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Group)
                .WithMany(g => g.GroupPermissions)
                .HasForeignKey(gp => gp.GroupId);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Permission)
                .WithMany(p => p.GroupPermissions)
                .HasForeignKey(gp => gp.PermissionId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);

            modelBuilder.Entity<Permission>()
                .HasData(
                    new Permission { Id = 1, Name = "system:create" },
                    new Permission { Id = 2, Name = "system:update" },
                    new Permission { Id = 3, Name = "system:delete" },
                    new Permission { Id = 4, Name = "system:read" }
                );

            modelBuilder.Entity<Group>()
                .HasData(
                    new Group { Id = 1, Name = "SuperAdmin" }
                );

            modelBuilder.Entity<GroupPermission>()
                .HasData(
                    new GroupPermission { GroupId = 1, PermissionId = 1 },
                    new GroupPermission { GroupId = 1, PermissionId = 2 },
                    new GroupPermission { GroupId = 1, PermissionId = 3 },
                    new GroupPermission { GroupId = 1, PermissionId = 4 }
                );

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Username = "sa",
                        PasswordHash = "$2a$11$7ptEF7sNj7VdOOE3zZsDaO/SfcU9VSrWID9npqGaSz3tIIkvwuDNm",
                        Email = "sa@dev.com"
                    }
                );

            modelBuilder.Entity<UserGroup>()
                .HasData(
                    new UserGroup { UserId = 1, GroupId = 1 }
                );
        }
    }
}