using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolWebApi.AppMetrics
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppMetricsService
    {
        private static IConfigurationSection AppMetricsSection
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var config = builder.Build();
                return config.GetSection("AppMetrics");
            }
        }

        /// <summary>
        /// 添加AppMetrics服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAppMetrics(this IServiceCollection services)
        {
            bool isOpenMetrics = Convert.ToBoolean(AppMetricsSection.GetSection("IsOpen").Value);
            if (isOpenMetrics)
            {
                var metrics = App.Metrics.AppMetrics.CreateDefaultBuilder().Configuration.Configure(options =>
                {
                    options.AddAppTag(AppMetricsSettings.App);
                    options.AddEnvTag(AppMetricsSettings.Env);
                }).Report.ToInfluxDb(options =>
                {
                    options.InfluxDb.BaseUri = new Uri(AppMetricsSettings.ConnectionString);
                    options.InfluxDb.Database = AppMetricsSettings.DatabaseName;
                    options.InfluxDb.UserName = AppMetricsSettings.UserName;
                    options.InfluxDb.Password = AppMetricsSettings.Password;
                    options.HttpPolicy.BackoffPeriod = AppMetricsSettings.BackoffPeriod;
                    options.HttpPolicy.FailuresBeforeBackoff = AppMetricsSettings.FailuresBeforeBackoff;
                    options.HttpPolicy.Timeout = AppMetricsSettings.Timeout;
                    options.FlushInterval = AppMetricsSettings.FlushInterval;
                }).Build();

                services.AddMetrics(metrics);
                services.AddMetricsReportScheduler();
                services.AddMetricsTrackingMiddleware(option => new MetricsWebTrackingOptions
                {
                    ApdexTrackingEnabled = true,
                    ApdexTSeconds = 0.1,
                    IgnoredHttpStatusCodes = new List<int> { 404 },
                    IgnoredRoutesRegexPatterns = new List<string>(),
                    OAuth2TrackingEnabled = true
                });
                services.AddMetricsEndpoints(option => new MetricEndpointsOptions
                {
                    MetricsEndpointEnabled = true,
                    MetricsTextEndpointEnabled = true,
                    EnvironmentInfoEndpointEnabled = true
                });
            }
        }
    }
}
