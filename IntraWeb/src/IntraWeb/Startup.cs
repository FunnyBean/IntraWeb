using IntraWeb.Models;
using IntraWeb.Services.Email;
using IntraWeb.Services.Template;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IntraWeb.ViewModels.Rooms;
using System.Net;
using System;
using IntraWeb.Models.Rooms;
using System.Threading.Tasks;
using AutoMapper;

namespace IntraWeb
{
    public class Startup
    {

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

            services.AddIdentity<ApplicationUser, IdentityRole>(conf =>
            {
                //ToDo: Refaktorovat. Extrahovat do zvlast triedy, ked bude jasne ako ideme riesit autorizaciu.
                conf.Password.RequiredLength = 8;
                conf.Password.RequireNonLetterOrDigit = false;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int) HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                };
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc(); // ToDo: Replace with Web API when it will be done in ASP.NET Core 1.0

            // Add application services
            AddIntraWebServices(services);

            //services.AddInstance<IRoomRepository>(new Models.Dummies.RoomDummyRepository()); //Testovacia implementacia
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
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
                catch (Exception e)
                {
                }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc();
        }


        private static void InitializeAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomsMappingProfile>();
            });

            services.AddInstance<IMapper>(config.CreateMapper());
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
        }


        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

    }
}