using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Configures and builds the application host with default settings and logging configuration.
        /// </summary>
        /// <param name="args">Command-line arguments passed during application startup.</param>
        /// <returns>
        /// An <see cref="IHostBuilder"/> configured with the default settings for the web application, 
        /// including logging setup using NLog from the configuration.
        /// </returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddNLog(hostingContext.Configuration.GetSection("Logging"));
                    });
                });
    }
}
