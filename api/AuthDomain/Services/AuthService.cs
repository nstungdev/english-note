using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthDomain.Data;
using api.Common.Models;
using Microsoft.Extensions.Options;
using api.AuthDomain.Options;
using api.AuthDomain.DTOs;

namespace api.AuthDomain.Services;

public class AuthService(
	ILogger<AuthService> logger,
	AppDbContext dbContext,
	IOptions<JwtOption> jwtOptionConfigure)
{
	readonly JwtOption jwtOption = jwtOptionConfigure.Value;
	public async Task<ApiResponse> RegisterUser(RegisterRequest register)
	{
		logger.LogInformation("Registering user: {Username}", register.Username);

		// Check if the user already exists
		var existingUser = await dbContext.Users
			.AnyAsync(u => u.Username == register.Username || u.Email == register.Email);
		if (existingUser)
		{
			return ApiResponse.ErrorResponse(
				message: "Username or email already exists.",
				statusCode: 400);
		}

		var hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);

		var user = new User
		{
			Username = register.Username,
			Email = register.Email,
			PasswordHash = hashedPassword,
			FullName = register.FullName,
			CreatedAt = DateTime.UtcNow
		};

		await dbContext.Users.AddAsync(user);
		await dbContext.SaveChangesAsync();

		return ApiResponse.SuccessResponse(
			data: new { user.Id, user.Username, user.Email },
			message: "User registered successfully.",
			statusCode: 201);
	}

	public async Task<ApiResponse> Login(LoginRequest request)
	{
		var user = await dbContext.Users
			.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail
				|| u.Email == request.UsernameOrEmail);

		if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
		{
			return ApiResponse.ErrorResponse("Invalid username/email or password.");
		}
		var accessToken = GenerateJwtToken(user);
		var refreshToken = GenerateRefreshToken(user);
		return ApiResponse.SuccessResponse(
			data: new
			{
				user.Id,
				user.Username,
				user.Email,
				AccessToken = accessToken,
				RefreshToken = refreshToken,
			},
			message: "Login successful.",
			statusCode: 200);
	}

	public async Task<ApiResponse> RefreshToken(string token)
	{
		var refreshToken = await dbContext.RefreshTokens
			.Include(rt => rt.User)
			.FirstOrDefaultAsync(rt => rt.Token == token);

		if (refreshToken == null
			|| refreshToken.ExpiryDate <= DateTime.UtcNow
			|| refreshToken.IsRevoked)
		{
			return ApiResponse.ErrorResponse(
				message: "Invalid or expired refresh token.",
				statusCode: 401);
		}

		// Revoke the old refresh token
		refreshToken.IsRevoked = true;
		dbContext.RefreshTokens.Update(refreshToken);

		// Generate new tokens
		var newAccessToken = GenerateJwtToken(refreshToken.User);
		var newRefreshToken = GenerateRefreshToken(refreshToken.User);

		await dbContext.SaveChangesAsync();

		return ApiResponse.SuccessResponse(
			data: new
			{
				refreshToken.User.Id,
				refreshToken.User.Username,
				refreshToken.User.Email,
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken,
			},
			message: "Tokens refreshed successfully.",
			statusCode: 200);
	}

	public async Task<ApiResponse> RevokeRefreshToken(string token)
	{
		var refreshToken = await dbContext.RefreshTokens
			.FirstOrDefaultAsync(rt => rt.Token == token);

		if (refreshToken == null)
		{
			return ApiResponse.ErrorResponse("Invalid refresh token.");
		}

		refreshToken.IsRevoked = true;
		dbContext.RefreshTokens.Update(refreshToken);
		await dbContext.SaveChangesAsync();
		return ApiResponse.SuccessResponse(
			message: "Refresh token revoked successfully.",
			statusCode: 200);
	}

	private async Task<IEnumerable<Claim>> ResolveUserPermissions(User user)
	{
		var userPermissions = await dbContext.UserPermissions
			.Where(up => up.UserId == user.Id)
			.Select(up => up.Permission.Name)
			.ToListAsync();

		var groupPermissions = await dbContext.UserGroups
			.Where(ug => ug.UserId == user.Id)
			.SelectMany(ug => ug.Group.GroupPermissions)
			.Select(gp => gp.Permission.Name)
			.ToListAsync();

		var allPermissions = userPermissions.Union(groupPermissions).Distinct();

		return allPermissions.Select(permission => new Claim("permission", permission));
	}

	private async Task<string> GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(jwtOption.Key);
		var userPermissions = await ResolveUserPermissions(user);

		var claims = new List<Claim>
		{
			new (ClaimTypes.NameIdentifier, user.Id.ToString()),
			new (ClaimTypes.Name, user.Username),
			new (ClaimTypes.Email, user.Email),
			new (
				Constants.PermissionsClaimType,
				string.Join(",", userPermissions))
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(15),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private string GenerateRefreshToken(User user)
	{
		var refreshToken = new RefreshToken
		{
			UserId = user.Id,
			Token = Guid.NewGuid().ToString(),
			ExpiryDate = DateTime.UtcNow.AddDays(7),
			CreatedByIp = "127.0.0.1" // Placeholder for IP address
		};

		dbContext.RefreshTokens.Add(refreshToken);
		dbContext.SaveChanges();

		return refreshToken.Token;
	}
}
