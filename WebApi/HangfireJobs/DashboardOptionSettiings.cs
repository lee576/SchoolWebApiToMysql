using System.IO;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SchoolWebApi.HangfireJobs
{
    /// <summary>
    /// Dashboard设置选项帮助类
    /// </summary>
    public class DashboardOptionSettiings
    {
        /// <summary>
        /// HangfireDashboard配置文件节
        /// </summary>
        private static IConfigurationSection DashboardSection
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var config = builder.Build();
                return config.GetSection("HangfireDashboard");
            }
        }

        /// <summary>
        /// Dashboard登录用户名
        /// </summary>
        public static string UserName => DashboardSection.GetSection("UserName").Value;

        /// <summary>
        /// Dashboard登录密码
        /// </summary>
        public static string Password => DashboardSection.GetSection("Password").Value;

        /// <summary>
        /// Dashboard设置选项
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static DashboardOptions DashboardOptions(IHostingEnvironment env)
        {
            return new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(env,
                        new BasicAuthAuthorizationFilterOptions
                        {
                            RequireSsl = false,
                            SslRedirect = false,
                            LoginCaseSensitive = true,
                            Users = new[]
                            {
                                new BasicAuthAuthorizationUser
                                {
                                    Login = UserName,
                                    PasswordClear = Password
                                }
                            }
                        })
                }
            };
        }
    }
}
