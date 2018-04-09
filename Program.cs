using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using webkom.Helper;
using Microsoft.AspNetCore.Identity;
using webkom.Data;
using NHibernate;

namespace webkom
{
  public class Program
  {
    public static IConfigurationRoot Configuration { get; set; }
    public static int Main(string[] args)
    {
      
      var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");
      Configuration = builder.Build();
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .Destructure
          .ByTransforming<SqlParameter>(r=>new { ParameterName = r.ParameterName, Value = r.Value})
        .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
        .Enrich.FromLogContext()
        .WriteTo.MSSqlServer(Configuration.GetConnectionString("DefaultConnection"), "Logs")
        .WriteTo.RollingFile("logs\\log-{Date}.txt")
        .CreateLogger();
      try
      {
        Log.Information("Starting web host {0}", 1);
        var host = BuildWebHost(args);
        using (var scope = host.Services.CreateScope())
        {
          var services = scope.ServiceProvider;

          try
          {
            //EnsureDataStorageIsReady(services);
            //IdentityInicijalizacija.SeedData();
            //IdentityInicijalizacija.SeedMenu();
            //var init = new Seed(services.GetRequiredService<UserManager<ApplicationUser>>(), services.GetRequiredService<RoleManager<IdentityRole>>(), services.GetRequiredService<ISession>());
            var init = new Seed(services.GetRequiredService<KorisnikManager>(),  services.GetRequiredService<ISession>(), 
                                services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>());
            init.SeedDatabase();
            // init.SeedUsers();
            // init.SeedData("exec seed_meni");
            // init.SeedData("exec seed_ident");
            // init.SeedData("exec seed_subjekt");
            // init.SeedMenu();
            // init.SeedIdent();
            // init.SeedSubjekt();
          }
          catch (Exception ex)
          {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex.Message, "An error occurred while migrating the database.");
          }
        }
        host.Run();
        return 0;
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
        return 1;
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            //.ConfigureServices(services => services.AddAutofac())
            .UseSerilog()
            .UseStartup<Startup>()
            .Build();
  }
}
