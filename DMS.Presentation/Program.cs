using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.MapperHelper;
using DMS.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace DMS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5022); // HTTP port
                options.ListenAnyIP(44389, listenOptions => listenOptions.UseHttps()); // HTTPS port
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DMSContext>(op =>
            {
                op.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("defaultconn"));
            });

            builder.Services.AddAutoMapper(op => op.AddProfile(typeof(MappingProfile)));

            builder.Services.AddIdentity<AppUser, IdentityRole>(op=>
            {
                op.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<DMSContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IFolderService, FolderService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();
            builder.Services.AddScoped<ISharingService, SharingService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDashBoardService, DashBoardService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();


            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
            });

            builder.Services.AddScoped<ITrashService, TrashService>();        
            builder.Services.AddScoped<IStarredService, StarredService>();        

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
