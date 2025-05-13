using AuthenticationService.API.Application.Shared.Constant;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.API.Core.Jwt;

public interface IJwtService
{
    string GenerateToken(Guid userId, Guid sessionId, string email, int exp);
    string HashObject<T>(T obj);
    string GenerateRefreshToken();
}

public class JwtService : IJwtService
{
    private readonly byte[] _key;
    private readonly JsonWebTokenHandler _handler;

    public JwtService()
    {
        var SecretKey = JwtConst.JWT_SECRET_KEY;
        _key = Encoding.UTF8.GetBytes(SecretKey);
        _handler = new JsonWebTokenHandler();
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    public string GenerateToken(Guid userId, Guid sessionId, string email, int exp)
    {
        var key = new SymmetricSecurityKey(_key);

        var claims = new Dictionary<string, object>
        {
            [ClaimTypes.Email] = email,
            [ClaimTypes.Sid] = sessionId.ToString(),
            [ClaimTypes.NameIdentifier] = userId.ToString(),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                IssuedAt = null,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddSeconds(exp),
                Issuer = null,
                Audience = null,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
        
        _handler.SetDefaultTimesOnTokenCreation = false;

        var tokenString = _handler.CreateToken(tokenDescriptor);
        return tokenString;
    }

    public string HashObject<T>(T obj)
    {
        string json = JsonConvert.SerializeObject(obj);

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            StringBuilder hashString = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashString.Append(b.ToString("x2"));
            }

            return hashString.ToString();
        }
    }
}
