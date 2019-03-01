﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace MobileApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseMvc();

            string currentDirectory = string.Empty;
            if (env.IsDevelopment())
            {
                currentDirectory = Directory.GetCurrentDirectory();
            }
            else
            {
                dynamic type = (new Program()).GetType();
                currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
            }

            //设置首页
            string contentRoot = currentDirectory + @"\wwwroot\web";
            var options = new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(contentRoot, "card")),
                EnableDefaultFiles = true,
                StaticFileOptions = { ServeUnknownFileTypes = true },
                DefaultFilesOptions = { DefaultFileNames = new[] { "card.html" } }
            };
            app.UseFileServer(options);
        }
    }
}