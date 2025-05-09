using System.Security.Claims;
using System.Text;
using UserService.API.Application.Shared.Constant;

namespace UserService.API.Core.Jwt;

public interface IJwtService
{
    string GenerateToken(Guid userId, Guid sessionId, string email, int exp);
}

public class JwtService : IJwtService
{
    private readonly string DEFAULT_SECRET = "0ebe2440a9eba77bed3a7a081b9bb26d792baaec3fcac1eae95b7148bfdcb8c5";
    private readonly byte[] _key;
    private readonly JwtSecurityTokenHandler _handler;

    public JwtService()
    {
        var SecretKey = JwtConst.JWT_SECRET_KEY;
        _key = Encoding.ASCII.GetBytes(SecretKey);
        _handler = new JwtSecurityTokenHandler();
    }

    public string GenerateToken(Guid userId, Guid sessionId, string email, int exp)
    {
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") ?? DEFAULT_SECRET);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("sessionId", sessionId.ToString()),
                new("email", email)
            }),
            Issuer = userId.ToString(),
            Expires = DateTime.UtcNow.AddSeconds(exp),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _handler.CreateToken(tokenDescriptor);

        return _handler.WriteToken(token);
    }
}
