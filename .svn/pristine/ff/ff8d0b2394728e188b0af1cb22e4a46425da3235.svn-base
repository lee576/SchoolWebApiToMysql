using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using System.IO;

namespace SchoolWebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisHelper
    {
        /// <summary>
        /// 创建Redis客户端对象
        /// </summary>
        /// <returns></returns>
        public static RedisClient CreateClient()
        {
            var redisConnection = GetRedisConnectStr();
            var redisIP = redisConnection.Split(':')[0];
            var redisPort = int.Parse(redisConnection.Split(':')[1]);

            RedisClient redisClient = new RedisClient(redisIP, redisPort);
            return redisClient;
        }

        /// <summary>
        /// 获取Redis连接串
        /// </summary>
        /// <returns></returns>
        public static string GetRedisConnectStr()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();
            return config.GetConnectionString("RedisConnection");
        }
    }
}
