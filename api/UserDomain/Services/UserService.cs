using api.Common;
using api.Common.Data;
using api.Common.Models;
using api.UserDomain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace api.UserDomain.Services;

public class UserService(AppDbContext context)
{
    public async Task<ApiResponse> GetAsync()
    {
        var users = await context.Users
            .Include(u => u.UserGroups)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Groups = u.UserGroups.Select(ug => ug.Group.Name),
                IsBlocked = u.IsBlocked
            })
            .ToArrayAsync();
        return ApiResponse.SuccessResponse(data: users);
    }

    public async Task<ApiResponse> UpdatePermissionsAsync(int id, UpdateUserPermissionsRequest request)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return ApiResponse.ErrorResponse(message: "User not found.", statusCode: 404);
        }

        // get group permissions by user
        var permsFromGroup = await context.UserGroups
            .Include(ug => ug.Group)
            .ThenInclude(g => g.GroupPermissions)
            .ThenInclude(gp => gp.Permission)
            .Where(ug => ug.UserId == id)
            .Select(e => e.Group.GroupPermissions.Select(gp => gp.Permission.Id))
            .ToListAsync();

        // get permission by user and groups
        var permsFromUser = await context.UserPermissions
            .Include(up => up.Permission)
            .Where(up => up.UserId == id)
            .Select(up => up.Permission.Id)
            .ToListAsync();

        var allGrantedPermIds = permsFromGroup.SelectMany(g => g).Union(permsFromUser).Distinct();

        // check input permissions are exist in all permissions
        var ungrantedPermissions = request.PermissionIds.Except(allGrantedPermIds);
        if (!ungrantedPermissions.Any())
        {
            return ApiResponse.ErrorResponse(
                message: "All of permisison are granted.", statusCode: 400);
        }

        using var transaction = await context.Database.BeginTransactionAsync();
        foreach (var permissionId in ungrantedPermissions)
        {
            var permissionEntity = await context.Permissions
                .FirstOrDefaultAsync(p => p.Id == permissionId);
            if (permissionEntity == null)
            {
                await transaction.RollbackAsync();
                return ApiResponse.ErrorResponse(
                    message: "Permission one of permission not found.", statusCode: 404);
            }

            var userPermission = new UserPermission
            {
                UserId = id,
                PermissionId = permissionEntity.Id
            };
            await context.UserPermissions.AddAsync(userPermission);
        }
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return ApiResponse.SuccessResponse(message: "Permissions updated successfully.");
    }

    public async Task<ApiResponse> BlockAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return ApiResponse.ErrorResponse(message: "User not found.", statusCode: 404);
        }

        user.IsBlocked = true;
        await context.SaveChangesAsync();
        return ApiResponse.SuccessResponse(message: "User blocked successfully.");
    }
}