using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using EcommApiCoreV3.BAL;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.JWT;
using EcommApiCoreV3.Repository;
using EcommApiCoreV3.Repository.Interface;
using EcommApiCoreV3.Services;
using EcommApiCoreV3.Utility;
using DinkToPdf.Contracts;
using DinkToPdf;

namespace EcommApiCoreV3
{
    public class Startup
    {
        //public static string UsesmtpSSL
        //{
        //    get;
        //    private set;
        //}
        //public static string EnableSsl
        //{
        //    get;
        //    private set;
        //}
        //public static string enableMail
        //{
        //    get;
        //    private set;
        //}
        public static string mailServer
        {
            get;
            private set;
        }
        public static string userId
        {
            get;
            private set;
        }
        public static string password
        {
            get;
            private set;
        }

        //public static string authenticate
        //{
        //    get;
        //    private set;
        //}

        //public static string AdminEmailID
        //{
        //    get;
        //    private set;
        //}

        public static string fromEmailID
        {
            get;
            private set;
        }

        //public static string DomainName
        //{
        //    get;
        //    private set;
        //}

        //public static string AllowSendMails
        //{
        //    get;
        //    private set;
        //}

        //public static string UserName
        //{
        //    get;
        //    private set;
        //}
        public static string WebSiteURL
        {
            get;
            private set;
        }
        public Startup(IHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json").Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddControllers();
            //Inject AppSettings
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            //services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            //{
            //    builder
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials()
            //    .AllowAnyOrigin();
            //    //.WithOrigins("http://ecom.uccnoida.com", "http://adminecom.uccnoida.com", "http://localhost:4100", "http://localhost:4200", "http://localhost:4000", "http://localhost:4300", "http://localhost:5000", "http://localhost:5100", "http://localhost:5200", "http://localhost:4500", "http://localhost:4800", "http://localhost:5300");
            //}));

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin",
                    options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
            //services.AddControllers();

            services.AddMvc();

            services.AddTransient<ICategoryBAL, CategoryBAL>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddTransient<ISubCategoryBAL, SubCategoryBAL>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();

            services.AddTransient<IBrandBAL, BrandBAL>();
            services.AddScoped<IBrandRepository, BrandRepository>();

            services.AddTransient<IProductBAL, ProductBAL>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddTransient<ISupplierBAL, SupplierBAL>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            services.AddTransient<IUsersBAL, UsersBAL>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddTransient<ILookupBAL, LookupBAL>();
            services.AddScoped<ILookupRepository, LookupRepository>();

            services.AddTransient<IFabricBAL, FabricBAL>();
            services.AddScoped<IFabricRepository, FabricRepository>();

            services.AddTransient<ILookupTagBAL, LookupTagBAL>();
            services.AddScoped<ILookupTagRepository, LookupTagRepository>();

            services.AddTransient<ICartBAL, CartBAL>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddTransient<IBillingAddressBAL, BillingAddressBAL>();
            services.AddScoped<IBillingAddressRepository, BillingAddressRepository>();

            services.AddTransient<IOrderBAL, OrderBAL>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddTransient<IEmailTemplateBAL, EmailTemplateBAL>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();

            services.AddTransient<IAgentBAL, AgentBAL>();
            services.AddScoped<IAgentRepository, AgentRepository>();

            services.AddTransient<ITransportBAL, TransportBAL>();
            services.AddScoped<ITransportRepository, TransportRepository>();

            services.AddScoped<UserService>();
            services.AddHttpContextAccessor();
            //*************************JWT Authentication Start here****************************


            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:Jwt_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

            services.AddScoped<IAuthorizeService, AuthorizeService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //*************************JWT Authentication End here****************************
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
             );
            app.UseRouting();

            app.UseAuthentication();//JWT Authentication
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseStaticFiles(); // For the wwwroot folder

            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "Status code page, status code: " +
                    context.HttpContext.Response.StatusCode);
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImage")),
                RequestPath = "/ProductImage"

            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImage")),
                RequestPath = "/ProductImage",
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Json")),
                RequestPath = "/Json"
            });
            //app.UseFileServer(new FileServerOptions()
            //{
            //    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
            //       @"C:\\Project\\uccApiCore\\uccApiCore2\\wwwroot\\ProductImage"),
            //    RequestPath = "/ProductImage",
            //    EnableDirectoryBrowsing = true
            //});

            //app.UseCors(options => options
            //.AllowAnyOrigin()
            //.AllowAnyHeader()
            //.AllowAnyMethod());
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<NotifyHub>("/notify");
            //});
            //app.UseMvc();
            //UsesmtpSSL = Configuration["EmailSetting:UsesmtpSSL"];
            //EnableSsl = Configuration["EmailSetting:EnableSsl"];
            //enableMail = Configuration["EmailSetting:enableMail"];

            mailServer = Configuration["EmailSetting:mailServer"];
            userId = Configuration["EmailSetting:userId"];
            password = Configuration["EmailSetting:password"];
            fromEmailID = Configuration["EmailSetting:fromEmailID"];

            //authenticate = Configuration["EmailSetting:authenticate"];
            //AdminEmailID = Configuration["EmailSetting:AdminEmailID"];

            //DomainName = Configuration["EmailSetting:DomainName"];
            //AllowSendMails = Configuration["EmailSetting:AllowSendMails"];
            //UserName = Configuration["EmailSetting:UserName"];
            WebSiteURL = Configuration["EmailSetting:WebSiteURL"];
        }
    }

}
