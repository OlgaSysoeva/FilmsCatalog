using FilmsCatalog.Data;
using FilmsCatalog.Models;
using FilmsCatalog.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FilmsCatalog.Repositories;
using Microsoft.Extensions.FileProviders;
using System.IO;
using AutoMapper;

namespace FilmsCatalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string aspConnectionString = Configuration.GetConnectionString("AspConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(aspConnectionString));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            string DbConnectionString = Configuration.GetConnectionString("DbConnection");
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(DbConnectionString));

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped(typeof(IDatabaseRepository<>), typeof(DatabaseRepository<>));

            services.AddDatabaseDeveloperPageExceptionFilter();            
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.Configure<FilesConfigModel>(Configuration.GetSection("Files"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "node_modules")
                ),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
