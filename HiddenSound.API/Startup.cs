﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HiddenSound.API.Auth;
using HiddenSound.API.Configuration;
using HiddenSound.API.Controllers;
using HiddenSound.API.Filters;
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
                options.AddPolicy("Application", 
                    b => b.AllowAnyOrigin()
                            .AllowAnyMethod());

                options.AddPolicy("API",
                    b => b.AllowAnyOrigin()
                            .AllowAnyMethod());

                options.AddPolicy("OAuth",
                    b => b.AllowAnyOrigin().AllowAnyMethod());
            });

            services.AddIdentity<HiddenSoundUser, HiddenSoundRole>()
                .AddEntityFrameworkStores<HiddenSoundDbContext, int>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict<int>()
                .AddEntityFrameworkCoreStores<HiddenSoundDbContext>()
                .AddMvcBinders()
                .EnableAuthorizationEndpoint("/OAuth/Authorize")
                .EnableTokenEndpoint($"/{OAuthConstants.ControllerRoute}/{OAuthConstants.TokenRoute}")
                .EnableLogoutEndpoint("/OAuth/Logout")
                .EnableUserinfoEndpoint($"/Api/{OAuthConstants.UserInfoRoute}")
                .AllowAuthorizationCodeFlow()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()
                .EnableRequestCaching()
                .DisableHttpsRequirement();

            services.AddMvc(options =>
            {
                options.Filters.Add(new TypeFilterAttribute(typeof(GlobalAuthenticationFilter)));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HiddenSound API", Version = "v1" });

                c.OperationFilter<ReplaceTagOperationFilter>();
            });

            services.Configure<SendGridConfig>(Configuration.GetSection("ThirdParty:SendGrid"));

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            var builder = new ContainerBuilder();
            var manager = new ApplicationPartManager();

            manager.ApplicationParts.Add(new AssemblyPart(typeof(ValuesController).GetTypeInfo().Assembly));
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
