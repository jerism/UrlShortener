using System;
using System.IO;
using System.Threading.Tasks;
using Database;
using Database.Interfaces;
using Database.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrlShortener.Services;
using UrlShortener.Utilities;

namespace UrlShortener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var databasePath = Path.Join(path, "urlshortener.db");
            services.AddDbContext<UrlShortenerContext>(options => options.UseSqlite($"Data Source={databasePath}"));
            services.AddRazorPages();
            services.AddTransient<IUrlService, UrlService>();
            services.AddTransient<IUrlRepository, UrlRepository>();
            services.AddTransient<GenerateShortenedUrl, GenerateShortenedUrl>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapGet("/", context =>
                {
                    return Task.Run(() => context.Response.Redirect("/Home/Index"));
                });
            });

            // Apply migrations to DB
            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using var serviceScope = scopeFactory.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<UrlShortenerContext>();

            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                context.Database.Migrate();
            }
        }
    }
}
