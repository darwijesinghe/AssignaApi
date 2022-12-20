using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AssignaApi.Data
{
    // database options
    public class DatabaseOptions
    {
        public int commandTimeout { get; set; }
        public string connectionString { get; set; } = string.Empty;
        public bool enableDetailedErrors { get; set; }
        public int maxRetryCount { get; set; }
    }

    public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
    {
        // services
        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            var connection = _configuration.GetConnectionString("DefaultConnection");
            options.connectionString = connection;

            // bind rest of values
            _configuration.GetSection("DatabaseOptions").Bind(options);
        }
    }
}