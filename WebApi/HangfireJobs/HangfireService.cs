using Hangfire;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SchoolWebApi.HangfireJobs
{
    /// <summary>
    /// 注册Hangfire服务
    /// </summary>
    public static class HangfireService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddHangfireService(this IServiceCollection services)
        {
            var redis =  ConnectionMultiplexer.Connect(RedisHelper.GetRedisConnectStr());
            services.AddHangfire(
                x =>
                {
                    x.UseRedisStorage(redis);
                    x.UseConsole();
                    //x.UseRecurringJob("recurringjob.json");
                    x.UseRecurringJob(typeof(RecurringJobService));
                    x.UseDefaultActivator();
                });
        }
    }
}
