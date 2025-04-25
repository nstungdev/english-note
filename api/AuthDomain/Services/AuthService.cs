using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using api.Common.Data;
using api.Common.Models;
using Microsoft.Extensions.Options;
using api.AuthDomain.Options;
using api.AuthDomain.DTOs;
using api.Common;

namespace api.AuthDomain.Services;

public class AuthService(
	ILogger<AuthService> logger,
	AppDbContext dbContext,
	IOptions<JwtOption> jwtOptionConfigure)
{
	readonly JwtOption jwtOption = jwtOptionConfigure.Value;
	public async Task<ApiResponse> RegisterUserAsync(RegisterRequest register)
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
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow,
			IsBlocked = false
		};

		await dbContext.Users.AddAsync(user);
		await dbContext.SaveChangesAsync();

		return ApiResponse.SuccessResponse(
			data: new { user.Id, user.Username, user.Email },
			message: "User registered successfully.",
			statusCode: 201);
	}

	public async Task<ApiResponse> LoginAsync(LoginRequest request)
	{
		logger.LogInformation("Logging in user: {UsernameOrEmail}", request.UsernameOrEmail);
		var user = await dbContext.Users
			.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail
				|| u.Email == request.UsernameOrEmail);

		if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
		{
			return ApiResponse.ErrorResponse(
				message: "Invalid username/email or password.",
				statusCode: 401);
		}

		if (user.IsBlocked)
		{
			return ApiResponse.ErrorResponse(
				message: "User is blocked.",
				statusCode: 403);
		}

		var accessToken = await GenerateJwtTokenAsync(user);
		var refreshToken = await GenerateRefreshTokenAsync(user);
		return ApiResponse.SuccessResponse(
			data: new AuthResponse
			{
				UserId = user.Id,
				Username = user.Username,
				Email = user.Email,
				AccessToken = accessToken,
				RefreshToken = refreshToken,
			},
			message: "Login successful.",
			statusCode: 200);
	}

	public async Task<ApiResponse> RefreshTokenAsync(string token)
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
		var newAccessToken = await GenerateJwtTokenAsync(refreshToken.User);
		var newRefreshToken = await GenerateRefreshTokenAsync(refreshToken.User);

		await dbContext.SaveChangesAsync();

		return ApiResponse.SuccessResponse(
			data: new AuthResponse
			{
				UserId = refreshToken.User.Id,
				Username = refreshToken.User.Username,
				Email = refreshToken.User.Email,
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken,
			},
			message: "Tokens refreshed successfully.",
			statusCode: 200);
	}

	public async Task<ApiResponse> RevokeRefreshTokenAsync(string token)
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

	private async Task<IEnumerable<Claim>> ResolveUserPermissionsAsync(User user)
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

		return allPermissions.Select(permission => new Claim(Constants.PermissionsClaimType, permission));
	}

	private async Task<string> GenerateJwtTokenAsync(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(jwtOption.Key);
		var userPermissions = await ResolveUserPermissionsAsync(user);

		var claims = new List<Claim>
		{
			new (ClaimTypes.NameIdentifier, user.Id.ToString()),
			new (ClaimTypes.Name, user.Username),
			new (ClaimTypes.Email, user.Email),
		};
		claims.AddRange(userPermissions);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddHours(jwtOption.TokenLifetimeHours),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private async Task<string> GenerateRefreshTokenAsync(User user)
	{
		var refreshToken = new RefreshToken
		{
			UserId = user.Id,
			Token = Guid.NewGuid().ToString(),
			ExpiryDate = DateTime.UtcNow.AddDays(jwtOption.RefreshTokenLifetimeDays),
			CreatedByIp = "127.0.0.1" // Placeholder for IP address
		};

		await dbContext.RefreshTokens.AddAsync(refreshToken);
		await dbContext.SaveChangesAsync();

		return refreshToken.Token;
	}
}
