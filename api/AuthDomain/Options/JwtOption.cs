using System;

namespace api.AuthDomain.Options;

public record JwtOption
{
	public required string Key { get; set; }
}
