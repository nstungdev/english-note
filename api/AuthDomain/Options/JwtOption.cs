using System;

namespace api.AuthDomain.Options;

public record JwtOption
{
	public required string Key { get; set; }
	public int TokenLifetimeHours { get; set; } = 8;
	public int RefreshTokenLifetimeDays { get; set; } = 7;
}
