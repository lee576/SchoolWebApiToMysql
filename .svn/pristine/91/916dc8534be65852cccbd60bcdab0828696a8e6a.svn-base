using Alipay.AopSdk.AspnetCore;
using AutoMapper;
using Exceptionless;
using Hangfire;
using LogDashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using SchoolWebApi;
using SchoolWebApi.AppMetrics;
using SchoolWebApi.HangfireJobs;
using SchoolWebApi.MiddleWare;
using SchoolWebApi.SwaggerExtension;
using SchoolWebApi.Utility;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private const string SWAGGER_ATUH_COOKIE = nameof(SWAGGER_ATUH_COOKIE);

        /// <summary>
        /// 配置文件接口
        /// </summary>
        public IConfiguration Configuration { get; }
        private IServiceCollection _services;

        private void ConfigureAlipay(IServiceCollection services)
        {
            var alipayOptions = Configuration.GetSection("Alipay").Get<AlipayOptions>();
            //检查RSA私钥
            AlipayConfigChecker.Check(alipayOptions.SignType, alipayOptions.PrivateKey);
            _services = services;
        }

        /// <inheritdoc />
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISchemaRegistryFactory, SchemaRegistryFactory>();
            services.AddScoped<JsonSerializerSettings>();
            services.AddScoped<SpireDocHelper>();

            //配置alipay服务
            ConfigureAlipay(services);
            services.AddSingleton<IQRCode, RaffQRCode>();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddOptions();
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.AddHttpClient("HttpClientFactory");

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                //Json 保持大小写原样
                .AddJsonOptions(op =>
                    op.SerializerSettings.ContractResolver =
                        new DefaultContractResolver())
                //Json 日期格式统一为 yyyy-MM-dd HH:mm:ss，默认为  "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
                .AddJsonOptions(op =>
                    op.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");

            //加入日志面板
            services.AddLogDashboard();

            //注册Hangfire计划任务服务
            services.AddHangfireService();

            //注册文件操作工具类
            services.AddFileDI();

            //注册Jwt接口保护服务
            services.AddJwtService();

            //注册Swagger接口文档/调试服务
            services.AddSwaggerService();

            //注册App Metrics接口监控服务
            services.AddAppMetrics();

            //配置跨域处理
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();     //指定处理cookie
                });
            });

            //批量注册业务类服务
            foreach (var item in RefelctHelper.GetClassName("Service"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }

            //注册AutoMapper
            services.AddAutoMapper(typeof(Startup));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            #region App Metrics Api性能监控,要写在app.UseMvc前面
            bool isOpenMetrics = Convert.ToBoolean(Configuration["AppMetrics:IsOpen"]);
            if (isOpenMetrics)
            {
                app.UseMetricsAllEndpoints();
                app.UseMetricsAllMiddleware();
            }
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseCors("any");
            app.UserFileDI();

            //使用日志面板
            app.UseLogDashboard();

            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("nlog.config");

            #region 全局异常处理中间件
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            app.UseMvc(routes => { routes.MapRoute("default", "api/{controller}/{action}/{id?}"); });
            #endregion

            #region 异常日志收集框架Exceptionless
            ExceptionlessClient.Default.Configuration.ApiKey = Configuration.GetSection("Exceptionless:ApiKey").Value;
            ExceptionlessClient.Default.Configuration.ServerUrl = Configuration.GetSection("Exceptionless:ServerUrl").Value;
            ExceptionlessClient.Default.Configuration.UseInMemoryStorage();
            app.UseExceptionless();
            #endregion

            #region 启动Swagger
            var config = new TemplateServiceConfiguration();
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
            config.Language = Language.CSharp; 
            config.EncodedStringFactory = new RawStringFactory(); 
            config.EncodedStringFactory = new HtmlEncodedStringFactory(); 
            config.Debug = true;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (!env.IsDevelopment())
                {
                    //配置Swagger登录
                    HttpHeaderOperation.SwaggerMiddleWare.Authorization(app, c, SWAGGER_ATUH_COOKIE);
                }
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(-1);//不显示model
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.ShowExtensions();
                //css注入
                c.InjectStylesheet("/swagger-common.css");   //自定义样式
                c.InjectStylesheet("/buzyload/app.min.css"); //等待load遮罩层样式
                //js注入
                c.InjectJavascript("/jquery/jquery.js");     //jquery 插件
                c.InjectJavascript("/buzyload/app.min.js");  //loading 遮罩层js
                c.InjectJavascript("/swagger-lang.js");      //我们自定义的js
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campus API V1");
            });
            #endregion

            #region 启动Hangfire服务
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] { "default", "apis", "jobs" }
            });

            //启动Hangfire面板,调试模式不输入密码，访问线上服务器需要密码
            app.UseHangfireDashboard(
                pathMatch: "/hangfire",
                options: DashboardOptionSettiings.DashboardOptions(env));
            #endregion

            app.UseStaticFiles();

            #region 调试状态下列出所有注入的服务及生命周期
            if (env.IsDevelopment())
            {
                app.Map("/allservices", builder => builder.Run(async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync($"<h1>所有服务{_services.Count}个</h1><table><thead><tr><th>类型</th><th>生命周期</th><th>Instance</th></tr></thead><tbody>");
                    foreach (var svc in _services)
                    {
                        await context.Response.WriteAsync("<tr>");
                        await context.Response.WriteAsync($"<td>{svc.ServiceType.FullName}</td>");
                        await context.Response.WriteAsync($"<td>{svc.Lifetime}</td>");
                        await context.Response.WriteAsync($"<td>{svc.ImplementationType?.FullName}</td>");
                        await context.Response.WriteAsync("</tr>");
                    }
                    await context.Response.WriteAsync("</tbody></table>");
                }));
                app.UseDeveloperExceptionPage();
            }
            #endregion
        }
    }
}
