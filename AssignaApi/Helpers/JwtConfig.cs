using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AssignaApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using AssignaApi.Response;

namespace AssignaApi.Helpers
{
    public class JwtHelpers
    {
        // services
        private IHttpContextAccessor _httpContext;
        private readonly JwtConfig _jwtConfig;
        public JwtHelpers(IHttpContextAccessor httpContext, IOptions<JwtConfig> jwtConfig)
        {
            _httpContext = httpContext;
            _jwtConfig = jwtConfig.Value;
        }

        // JWT token generate
        public string GenerateJwtToken(string name, string mail, string role)
        {
            // claims
            var claims = new[]
            {
                new Claim("name", name),
                new Claim(JwtRegisteredClaimNames.Sub, mail),
                new Claim(JwtRegisteredClaimNames.Email, mail),
                new Claim("role", (role == Roles.lead) ? Roles.lead : Roles.member),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
            };

            // JWT secret
            var keyBytes = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
            var key = new SymmetricSecurityKey(keyBytes);

            // signing credentials
            var siCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // setup token
            var token = new JwtSecurityToken
            (
                audience: _jwtConfig.Audience,
                issuer: _jwtConfig.Issuer,
                claims: claims,
                notBefore: DateTime.Now.ToUniversalTime(),
                expires: DateTime.Now.ToUniversalTime().Add(TimeSpan.Parse(_jwtConfig.Expire)),
                signingCredentials: siCredentials
            );

            // write token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        // refresh token
        public string GenerateRandomToken(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var token = new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
            return token;
        }

        // read JWT token
        public AuthResult ReadJwtToken()
        {
            var claims = new string[2];

            // access token
            string authHeader = _httpContext.HttpContext.Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            string jwtToken = authHeader;

            // read token
            var handler = new JwtSecurityTokenHandler();
            string userName = handler.ReadJwtToken(jwtToken).Payload["name"].ToString() ?? string.Empty;
            string userRole = handler.ReadJwtToken(jwtToken).Payload["role"].ToString() ?? string.Empty;

            return new AuthResult
            {
                uesr_name = userName,
                role = userRole
            };
        }
    }

    public class JwtConfig
    {
        // JWT options
        public string Secret { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Expire { get; set; } = string.Empty;

    }
    public class JwtConfigSetup : IConfigureOptions<JwtConfig>
    {
        // services
        private readonly IConfiguration _configuration;
        public JwtConfigSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtConfig config)
        {
            // bind values
            _configuration.GetSection("JWTConfig").Bind(config);
        }

    }

}