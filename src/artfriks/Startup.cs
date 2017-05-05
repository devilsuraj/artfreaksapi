using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using artfriks.Data;
using artfriks.Models;
using artfriks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Models;
using OpenIddict.Core;
using CryptoHelper;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Serilog;
using AspNet.Security.OpenIdConnect.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace artfriks
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
         
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options => { 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
              }
            );

            /*  services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();*/


            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 3;
                o.Cookies.ApplicationCookie.AutomaticChallenge = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddOpenIddict()
               // Register the Entity Framework stores.
               .AddEntityFrameworkCoreStores<ApplicationDbContext>()
               // Register the ASP.NET Core MVC binder used by OpenIddict.
               // Note: if you don't call this method, you won't be able to
               // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
               .AddMvcBinders()
               // Enable the token endpoint (required to use the password flow).
               .UseJsonWebTokens()
               .EnableAuthorizationEndpoint("/connect/authorize")
               .EnableLogoutEndpoint("/connect/logout")
               .EnableTokenEndpoint("/connect/verifycode")
               .EnableUserinfoEndpoint("/Account/Userinfo")
               .AllowAuthorizationCodeFlow()
               .AllowRefreshTokenFlow()
               .AllowPasswordFlow()
               .AllowImplicitFlow()
              //Dont delete this line 9A403D79EAAC9915FDA1A28F7B5109390C5DCF06  DCBF6BC95C52BDE6AA1135297589A1ADB8BB7199
              .AddSigningCertificate("9A403D79EAAC9915FDA1A28F7B5109390C5DCF06", StoreName.My, StoreLocation.LocalMachine)
               .DisableHttpsRequirement()
               .EnableRequestCaching();
            //.AddEphemeralSigningKey();

            services.AddCors(options =>
            {
                options.AddPolicy("AeonPolicy",
                    builder => builder.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowCredentials()
                                     .WithExposedHeaders()
                                    .AllowAnyHeader());
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc();
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(env.ContentRootPath, "Logs/myapp-{Date}.txt"));
            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddSerilog();
            loggerFactory.AddDebug();
            app.UseCors("AeonPolicy");
            app.UseApplicationInsightsRequestTelemetry();
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseBrowserLink();
            app.UseApplicationInsightsExceptionTelemetry();
            app.UseStaticFiles();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience = false,
                    NameClaimType = OpenIdConnectConstants.Claims.Subject,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role
                },
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                Audience = "http://bo.artfreaksglobal.com/",
                Authority =  "http://bo.artfreaksglobal.com"
               // Authority="http://localhost:2822" 
            });
            app.UseOpenIddict();
            app.UseIdentity();
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


         
            using (var context = new ApplicationDbContext(
              app.ApplicationServices.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                var applications = context.Set<OpenIddictApplication>();

                // Add Mvc.Client to the known applications.
                if (!applications.Any())
                {
                    applications.Add(new OpenIddictApplication
                    {
                        ClientId = "myClient",
                        ClientSecret = Crypto.HashPassword("secret_secret_secret"),
                        DisplayName = "My client application",
                        LogoutRedirectUri = "http://bo.artfreaksglobal.com",
                        RedirectUri = "http://bo.artfreaksglobal.com/signin-oidc",
                        Type = OpenIddictConstants.ClientTypes.Public

                    });
                    applications.Add(new OpenIddictApplication
                    {
                        ClientId = "myClient2",
                        ClientSecret = Crypto.HashPassword("secret_secret_secret"),
                        DisplayName = "My client application",
                        LogoutRedirectUri = "http://54.201.192.104",
                        RedirectUri = "http://54.201.192.104/signin-oidc",
                        Type = OpenIddictConstants.ClientTypes.Confidential

                    });

                    // To test this sample with Postman, use the following settings:
                    //
                    // * Authorization URL: http://localhost:54540/connect/authorize
                    // * Access token URL: http://localhost:54540/connect/token
                    // * Client ID: postman
                    // * Client secret: [blank] (not used with public clients)
                    // * Scope: openid email profile roles
                    // * Grant type: authorization code
                    // * Request access token locally: yes
                    applications.Add(new OpenIddictApplication
                    {
                        ClientId = "postman",
                        DisplayName = "Postman",
                        RedirectUri = "https://www.getpostman.com/oauth2/callback",
                        Type = OpenIddictConstants.ClientTypes.Public
                    });
                    context.SaveChanges();
                  
                }
            }
        }
    }
}
