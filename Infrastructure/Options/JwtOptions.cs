
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Application.Options
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationInSeconds { get; set; }
        public int RefreshTokenExpirationInSeconds { get; set; }

        public JwtOptions(IConfiguration configuration)
        {
            SecretKey = configuration["Jwt:SecretKey"] ?? throw new ConfigurationErrorsException("Jwt:SecretKey is not configured");
            Issuer = configuration["Jwt:Issuer"] ?? throw new ConfigurationErrorsException("Jwt:Issuer is not configured");
            Audience = configuration["Jwt:Audience"] ?? throw new ConfigurationErrorsException("Jwt:Audience is not configured");
            AccessTokenExpirationInSeconds = int.TryParse(configuration["Jwt:AccessTokenExpirationInSeconds"], out int accessTokenExpiration) ? accessTokenExpiration : throw new ConfigurationErrorsException("Jwt:AccessTokenExpirationInSeconds is not configured or is not a valid integer");
            RefreshTokenExpirationInSeconds = int.TryParse(configuration["Jwt:RefreshTokenExpirationInSeconds"], out int refreshTokenExpiration) ? refreshTokenExpiration : throw new ConfigurationErrorsException("Jwt:RefreshTokenExpirationInSeconds is not configured or is not a valid integer");
        }
    }

}
