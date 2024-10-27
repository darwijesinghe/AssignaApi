using Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Configurations
{
    /// <summary>
    /// Class for sets up the configuration for JWT options
    /// </summary>
    public class JwtConfigSetup : IConfigureOptions<JwtConfig>
    {
        // Services
        private readonly IConfiguration _configuration;

        public JwtConfigSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtConfig config)
        {
            // binds values
            _configuration.GetSection("JWTConfig").Bind(config);
        }
    }
}
