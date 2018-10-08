using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NHibernate.Tool.hbm2ddl;
using Serilog;
using webkom.Helper;
using webkom.Models;

namespace webkom
{
    public class Startup
    {
        const string TokenAudience = "WebKomKorisnici";
        const string TokenIssuer = "WebKomApp";
        //        private Microsoft.IdentityModel.Tokens.RsaSecurityKey _key;
        private TokenAuthOptions _tokenOptions;
        private SymmetricSecurityKey _signingKey;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IHostingEnvironment env, IConfiguration config, ILoggerFactory loggerFactory)
        {
            _env = env;
            _config = config;
            _loggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CORS---------
            // services.AddCors(options =>
            // {
            //     options.AddPolicy("CorsPolicy",
            //         builder => builder.AllowAnyOrigin()
            //         .AllowAnyMethod()
            //         .AllowAnyHeader()
            //         .AllowCredentials());
            // });
            services.AddHttpContextAccessor();
            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
               .AddJsonOptions(opt =>
                  {
                      opt.SerializerSettings.ContractResolver = new NHibernateContractResolver();
                      opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                      //opt.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                      opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                      opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                  });
            var plainTextSecurityKey = "(Ne)tajnoviti tekst";
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));

            NHibernateHelper.ConnectionString = _config.GetConnectionString("DefaultConnection");

            services.AddSingleton(_config.GetSection("webKomKonfiguracija").Get<WebKomKonfiguracija>());

            var minutaToken = _config.GetSection("webKomKonfiguracija:minutaToken");
            var minutaRefreshToken = _config.GetSection("webKomKonfiguracija:minutaRefreshToken");
            _tokenOptions = new TokenAuthOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                Key = _signingKey,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature),
                MinutaToken = int.Parse(minutaToken.Value),
                MinuntaRefreshToken = int.Parse(minutaRefreshToken.Value)
                //,SecurityAlgorithms.HmacSha256Signature) // new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature) //, SecurityAlgorithms.Sha256Digest
            };
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            //});
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddSingleton(_tokenOptions);
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            //{
            //  config.Password.RequireNonAlphanumeric = false;
            //  config.Password.RequireDigit = false;
            //  config.Password.RequireLowercase = false;
            //  config.Password.RequiredLength = 3;
            //  config.Password.RequireUppercase = false;
            //})
            //.AddEntityFrameworkStores<ApplicationDbContext>()


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            //NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = _tokenOptions.Issuer,
                            ValidAudience = _tokenOptions.Audience,
                            IssuerSigningKey = _tokenOptions.Key //JwtSecurityKey.Create(plainTextSecurityKey)
                        };
                });

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddSingleton(factory =>
            {
                HttpHelper.Configure(factory.GetRequiredService<IHttpContextAccessor>(), factory.GetRequiredService<ILoggerFactory>());
                var config = FluentNHibernate.Cfg.Fluently.Configure()
                  .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.Driver<LoggerSqlClientDriver>()
                  .ConnectionString(_config.GetConnectionString("DefaultConnection")).ShowSql)
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Entitet>()
                  .Conventions.Add<CustomForeignKeyConvention>())
                  .ExposeConfiguration(cfg => cfg.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "web"))
                  .BuildConfiguration();
                //export
                var exporter = new SchemaExport(config);
                //exporter.Drop(true, true);
                //exporter.Create(true, true);
                //exporter.Execute(true, true, false);
                //update
                // var update = new SchemaUpdate(config);
                // update.Execute(true, true);

                config.SetInterceptor(new AuditInterceptor(factory.GetService<ILoggerFactory>(), NHibernateHelper.OpenSession(), factory.GetService<IHttpContextAccessor>()));
                config.BuildSessionFactory();//.SetInterceptor(new AuditInterceptor());

                return config.BuildSessionFactory();
            });

            services.AddScoped<NHibernate.ISession>(factory =>
               factory
                    .GetService<NHibernate.ISessionFactory>()
                    .OpenSession()
            );



            services.AddScoped(factory =>
              new KorisnikManager(factory.GetRequiredService<NHibernate.ISession>(),factory.GetRequiredService<IHttpContextAccessor>())
            );



            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
              .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser()
              .Build());
            });
            //    .AddMvcOptions(options =>
            //{

            //    options.Filters.Add(new RequireHttpsAttribute());
            //    options.SslPort = 443;
            //})

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseAuthentication();
            app.UseStaticFiles();
            //HttpHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>(), app.ApplicationServices.GetRequiredService<ILoggerFactory>());
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    //EnsureDataStorageIsReady(services);
                    
                    //var init = new Seed(serviceScope.ServiceProvider.GetService<KorisnikManager>(), serviceScope.ServiceProvider.GetService<NHibernate.ISession>(), serviceScope.ServiceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>());
                    //init.SeedDatabase();
                }
                catch (Exception ex)
                {
                    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex.Message, "An error occurred while migrating the database.");
                }

            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapSpaFallbackRoute(
                //    name: "spa-fallback",
                //    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
