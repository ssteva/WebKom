using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate;
using Serilog;
using Serilog.Events;
using webkom.Helper;

namespace webkom
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .Destructure
                .ByTransforming<SqlParameter>(r => new { r.ParameterName, r.Value })
              .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
              .Enrich.FromLogContext()
              //.WriteTo.MSSqlServer(Configuration.GetConnectionString("DefaultConnection"), "Logs")
              .WriteTo.RollingFile("logs\\log-{Date}.txt")
              .CreateLogger();
            try
            {
                Log.Information("Starting web host {0}", 1);
                var host = CreateWebHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    // try
                    // {
                    //   //EnsureDataStorageIsReady(services);
                    //   var init = new Seed(services.GetRequiredService<KorisnikManager>(), services.GetRequiredService<ISession>(),
                    //                       services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>());
                    //   init.SeedDatabase();
                    // }
                    // catch (Exception ex)
                    // {
                    //   var logger = services.GetRequiredService<ILogger<Program>>();
                    //   logger.LogError(ex.Message, "An error occurred while migrating the database.");
                    // }
                }
                host.Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
