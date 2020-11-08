namespace JwtLib
{
	using Microsoft.IdentityModel.Tokens;
	using System.Diagnostics;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Text;

	public class Analyze
	{
		public static bool ValidateToken(
			string token,
			string secret,
			string issuer = "",
			string audience = "")
		{
			var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(
					token,
					new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidIssuer = issuer,
						ValidAudience = audience,
						IssuerSigningKey = securityKey
					},
					out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}

			return true;
		}

		public static JwtSecurityToken CreateSecurityToken(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return null;
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

			return securityToken;
		}

		public static string GetClaim(string token, string claimType)
		{
			var securityToken = CreateSecurityToken(token);
			if (securityToken == null)
			{
				return null;
			}

			var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
			return stringClaimValue;
		}
	}
}
