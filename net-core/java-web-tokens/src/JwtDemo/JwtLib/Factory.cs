namespace JwtLib
{
	using Microsoft.IdentityModel.Tokens;
	using System;
	using System.IdentityModel.Tokens.Jwt;
	using System.Security.Claims;
	using System.Text;

	public static class Factory
	{
		public static string CreateToken(
			Claim[] claims,
			string secret,
			DateTime? expiresDateTime,
			string issuer = "",
			string audience = "")
		{
			var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = expiresDateTime, // In UTC time
				Issuer = issuer,
				Audience = audience,
				SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public static string CreateInfluxToken(
			string username,
			string secret,
			int daysValid = 7)
		{
			var expiresDateTime = DateTime.UtcNow.AddDays(daysValid);
			var offset = new DateTimeOffset(expiresDateTime);
			var unixTimeSpan = offset.ToUnixTimeSeconds();

			var claims = new Claim[] {
				new Claim(Helper.ClaimValueTypesUsername, username),
				new Claim(Helper.ClaimValueTypesExpiration, unixTimeSpan.ToString())
			};

			var result = CreateToken(claims, secret, expiresDateTime);
			return result;
		}
	}
}
