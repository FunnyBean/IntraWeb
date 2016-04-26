using AutoMapper;
using IntraWeb.Models;
using IntraWeb.Models.Rooms;
using IntraWeb.Models.Users;
using IntraWeb.Services.Email;
using IntraWeb.Services.Template;
using IntraWeb.ViewModels.Rooms;
using IntraWeb.ViewModels.Users;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace IntraWeb
{
    public class Startup
    {

        public const string AuthenticationScheme = "IntrawebAuthentication";

        IHostingEnvironment _env;


        public Startup(IHostingEnvironment env)
        {
            _env = env;

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<EmailOptions>(Configuration.GetSection("Email"));

            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));


            services.AddMvc(); // ToDo: Replace with Web API when it will be done in ASP.NET Core 1.0

            // Add application services
            AddIntraWebServices(services);
            InitializeAutoMapper(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                //loggerFactory.AddDebug(LogLevel.Verbose); - Log EF7 SQL Queries
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch (Exception)
                {
                }
            }

            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = AuthenticationScheme;
                options.CookieName = "IntraWebAuth";
                options.ReturnUrlParameter = "returnUrl";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/login";
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = false;
            });

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseIdentity();
            app.UseMvc();

            DbInitializer.Initialize(app.ApplicationServices);
        }

        private void AddIntraWebServices(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            services.AddScoped<IEmailCreator, HtmlEmailCreator>();
            services.AddScoped<ITemplateFormatter, TemplateFormatter>();
            services.AddScoped<ITemplateLoader, FileTemplateLoader>(
                (provider) => new FileTemplateLoader(System.IO.Path.Combine(_env.WebRootPath, "templates", "email"))
            );
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();

            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IUserRepository, UserDummyRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }

        private static void InitializeAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomsMappingProfile>();
                cfg.AddProfile<UsersMappingProfile>();
            });

            services.AddInstance<IMapper>(config.CreateMapper());
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

    }
}