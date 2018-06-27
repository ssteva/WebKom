using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using webkom.Data;
using webkom.Models;
using webkom.Helper;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using NHibernate.Tool.hbm2ddl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;


namespace webkom
{
    public class Startup
    {
        const string TokenAudience = "WebKomKorisnici";
        const string TokenIssuer = "WebKomApp";
        //        private Microsoft.IdentityModel.Tokens.RsaSecurityKey _key;
        private TokenAuthOptions _tokenOptions;
        private SymmetricSecurityKey _signingKey;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var plainTextSecurityKey = "(Ne)tajnoviti tekst";
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));

            NHibernateHelper.ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddSingleton(Configuration.GetSection("webKomKonfiguracija").Get<WebKomKonfiguracija>());

            var minutaToken = Configuration.GetSection("webKomKonfiguracija:minutaToken");
            var minutaRefreshToken = Configuration.GetSection("webKomKonfiguracija:minutaRefreshToken");
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireDigit = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequiredLength = 3;
                    config.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddSingleton(factory =>
            {
                var config = FluentNHibernate.Cfg.Fluently.Configure()
                  .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.Driver<LoggerSqlClientDriver>()
                  .ConnectionString(Configuration.GetConnectionString("DefaultConnection")).ShowSql)
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Models.Entitet>()
                  .Conventions.Add<CustomForeignKeyConvention>())
                  .ExposeConfiguration(cfg => cfg.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, "web"))
                  .BuildConfiguration();
                //export
                // var exporter = new SchemaExport(config);
                // exporter.Execute(true, true, false);
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
              new KorisnikManager(factory.GetRequiredService<NHibernate.ISession>())
            );



            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            services.AddMvc()
                //    .AddMvcOptions(options =>
                //{

                //    options.Filters.Add(new RequireHttpsAttribute());
                //    options.SslPort = 443;
                //})
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new NHibernateContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //opt.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            HttpHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>(), app.ApplicationServices.GetRequiredService<ILoggerFactory>());
            if (env.IsDevelopment())
            {

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
