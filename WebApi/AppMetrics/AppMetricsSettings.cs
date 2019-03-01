using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace SchoolWebApi.AppMetrics
{
    /// <summary>
    /// AppMetrics设置
    /// </summary>
    public class AppMetricsSettings
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
        /// InfluxDb DatabaseName
        /// </summary>
        public static string DatabaseName => AppMetricsSection.GetSection("DatabaseName").Value;
        /// <summary>
        /// InfluxDb ConnectionString
        /// </summary>
        public static string ConnectionString => AppMetricsSection.GetSection("ConnectionString").Value;
        /// <summary>
        /// 
        /// </summary>
        public static string App => AppMetricsSection.GetSection("App").Value;
        /// <summary>
        /// 
        /// </summary>
        public static string Env => AppMetricsSection.GetSection("Env").Value;
        /// <summary>
        /// InfluxDb UserName
        /// </summary>
        public static string UserName => AppMetricsSection.GetSection("UserName").Value;

        /// <summary>
        /// InfluxDb Password
        /// </summary>
        public static string Password => AppMetricsSection.GetSection("Password").Value;
        /// <summary>
        /// HttpPolicy BackoffPeriod
        /// </summary>
        public static TimeSpan BackoffPeriod => TimeSpan.FromSeconds(30);
        /// <summary>
        /// HttpPolicy FailuresBeforeBackoff
        /// </summary>
        public static int FailuresBeforeBackoff => 5;
        /// <summary>
        /// HttpPolicy Timeout
        /// </summary>
        public static TimeSpan Timeout => TimeSpan.FromSeconds(10);
        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan FlushInterval => TimeSpan.FromSeconds(5);
    }
}
