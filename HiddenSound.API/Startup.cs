using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HiddenSound.API.Configuration;
using HiddenSound.API.Identity;
using HiddenSound.API.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using OpenIddict.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace HiddenSound.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<SendGridConfig>(Configuration.GetSection("ThirdParty:SendGrid"));
            services.Configure<AppSettingsConfig>(Configuration.GetSection("AppSettings"));
            
            var connectionString = Configuration.GetConnectionString("HiddenSoundDatabase");
            services.AddDbContext<HiddenSoundDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

                options.UseOpenIddict<int>();
            });

            services.AddMemoryCache();
            services.AddOptions();

            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                options.AddPolicy("Application", p => p.WithOrigins(Configuration["AppSettings:WebUrl"]).AllowAnyMethod().AllowAnyHeader());
            });

            services.AddIdentity<HiddenSoundUser, HiddenSoundRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;

                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<HiddenSoundDbContext, int>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict<int>()
                .AddEntityFrameworkCoreStores<HiddenSoundDbContext>()
                .AddMvcBinders()
                .EnableAuthorizationEndpoint("/OAuth/Authorize")
                .EnableTokenEndpoint($"/{OAuthConstants.ControllerRoute}/{OAuthConstants.TokenRoute}")
                .EnableLogoutEndpoint("/OAuth/Logout")
                .EnableUserinfoEndpoint($"/{OAuthConstants.ControllerRoute}/{OAuthConstants.UserInfoRoute}")
                .AllowAuthorizationCodeFlow()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .EnableRequestCaching()
                .DisableHttpsRequirement();

            services.AddMvc(options =>
            {

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HiddenSound API", Version = "v1" });

                c.OperationFilter<ReplaceTagOperationFilter>();
            });


            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            var builder = new ContainerBuilder();
            var manager = new ApplicationPartManager();

            manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).GetTypeInfo().Assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());

            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance(); // not sure if actually needed any more
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();

            builder.RegisterModule<Module>();

            builder.Populate(services);
            
            ApplicationContainer = builder.Build();

            return ApplicationContainer.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            var sslPort = 0;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile(@"Properties/launchSettings.json", optional: false, reloadOnChange: true);

                var launchConfig = builder.Build();
                sslPort = launchConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.IsHttps)
                {
                    await next();
                }
                else
                {
                    var sslPortStr = sslPort == 0 || sslPort == 443 ? string.Empty : $":{sslPort}";
                    var httpsUrl = $"https://{ctx.Request.Host.Host}{sslPortStr}{ctx.Request.Path}";
                    ctx.Response.Redirect(httpsUrl);
                }
            });

            app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/Application"), branch =>
            {
                branch.UseCors("Application");
            });

            app.UseWhen(ctx => new[] { "/.well-known", "/Api", "/Mobile", "/OAuth" }.Any(p => ctx.Request.Path.StartsWithSegments(p)), branch =>
           {
               branch.UseCors("AnyOrigin");
           });

            app.UseOAuthValidation();

            app.UseOpenIddict();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HiddenSound v1");
                c.DocExpansion("none");
            });

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
            InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HiddenSoundDbContext>();
                await context.Database.EnsureCreatedAsync();

                var manager = services.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication<int>>>();

                if (await manager.FindByClientIdAsync("postman", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication<int>
                    {
                        ClientId = "postman",
                        DisplayName = "Postman",
                        RedirectUri = "https://www.getpostman.com/oauth2/callback"
                    };

                    await manager.CreateAsync(application, cancellationToken);
                }

                if (await manager.FindByClientIdAsync("angular2", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication<int>
                    {
                        ClientId = "angular2",
                        DisplayName = "Angular2",
                        RedirectUri = "http://localhost:52323",
                        LogoutRedirectUri = "http://localhost:52323"
                    };

                    await manager.CreateAsync(application, cancellationToken);
                }
            }
        }
    }
}
