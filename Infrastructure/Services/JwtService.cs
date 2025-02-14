using Application.Configurations;
using Application.General;
using Application.Interfaces.Services;
using Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    /// <summary>
    /// Service implementation class for IJwtService.
    /// </summary>
    public class JwtService : IJwtService
    {
        // Services
        private readonly JwtConfig            _jwtConfig;
        private readonly IHttpContextAccessor _httpContext;

        public JwtService(IHttpContextAccessor httpContext, IOptions<JwtConfig> jwtConfig)
        {
            _httpContext = httpContext;
            _jwtConfig   = jwtConfig.Value;
        }

        /// <summary>
        /// Generates a JWT token based on the user's name, email, and role.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="mail">The user's email address.</param>
        /// <param name="role">The user's role within the system.</param>
        /// <returns>
        /// Returns a JWT token as a string.
        /// </returns>
        public string GenerateJwtToken(string name, string mail, string role)
        {
            // claims
            var claims = new[]
            {
                new Claim("name"                       , name),
                new Claim(JwtRegisteredClaimNames.Sub  , mail),
                new Claim(JwtRegisteredClaimNames.Email, mail),
                new Claim("role"                       , (role == Role.Lead) ? Role.Lead : Role.Member),
                new Claim(JwtRegisteredClaimNames.Jti  , Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat  , DateTime.Now.ToString())
            };

            // Jwt secret
            var keyBytes = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
            var key      = new SymmetricSecurityKey(keyBytes);

            // signing credentials
            var siCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // expire time
            var minutes = _jwtConfig.ExpirationInMinutes;

            // setup token
            var token = new JwtSecurityToken
            (
                audience          : _jwtConfig.Audience,
                issuer            : _jwtConfig.Issuer,
                claims            : claims,
                notBefore         : DateTime.Now,
                expires           : DateTime.Now.AddMinutes(minutes),
                signingCredentials: siCredentials
            );

            // writes token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        /// <summary>
        /// Generates a random token of the specified length.
        /// </summary>
        /// <param name="length">The desired length of the random token.</param>
        /// <returns>
        /// Returns a randomly generated token as a string.
        /// </returns>
        public string GenerateRandomToken(int length)
        {
            // random obj
            var random = new Random();

            // random chars
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

            // generates the random token
            var token = new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());

            return token;
        }

        /// <summary>
        /// Creates both a JWT token and a refresh token based on the provided data.
        /// </summary>
        /// <param name="data">The data required to generate the tokens.</param>
        /// <returns>
        /// Returns a tuple containing the JWT token, refresh token, and the expiration time.
        /// </returns>
        public (string token, string refreshToken, int expireAt) MakeTokens(MakeToken data)
        {
            // main token
            var token        = GenerateJwtToken(data.Name, data.Mail, data.Role);

            // refresh token
            var refreshToken = GenerateRandomToken(data.Length);

            // expiration
            var expireAt     = _jwtConfig.ExpirationInMinutes;

            return (token, refreshToken, expireAt);
        }

        /// <summary>
        /// Reads the JWT token from the current context and extracts user details.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="AuthResponse"/> containing user authentication details.
        /// </returns>
        public AuthResponse ReadJwtToken()
        {
            // gets the token from context
            string authHeader = _httpContext.HttpContext.Request.Headers["Authorization"];
            authHeader        = authHeader.Replace("Bearer ", "");
            string jwtToken   = authHeader;

            // reads the token
            var handler     = new JwtSecurityTokenHandler();
            string userName = handler.ReadJwtToken(jwtToken).Payload["name"].ToString() ?? string.Empty;
            string userRole = handler.ReadJwtToken(jwtToken).Payload["role"].ToString() ?? string.Empty;

            return new AuthResponse
            {
                UserName = userName,
                Role     = userRole
            };
        }
    }
}
