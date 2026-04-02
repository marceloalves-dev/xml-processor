using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaxDocumentProcessor.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] LoginRequest request)
        {
            var expectedUsername = _configuration["Auth:Username"];
            var expectedPassword = _configuration["Auth:Password"];

            if (request.Username != expectedUsername || request.Password != expectedPassword)
                return Unauthorized();

            var secret = _configuration["Jwt:Secret"]!;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiration = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: [new Claim(ClaimTypes.Name, request.Username)],
                expires: DateTime.UtcNow.AddMinutes(expiration),
                signingCredentials: credentials
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

    public record LoginRequest(string Username, string Password);
}
