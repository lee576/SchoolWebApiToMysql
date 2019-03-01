using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Models.ViewModels;

namespace SchoolWebApi
{
    /// <summary>
    /// Jwt服务
    /// </summary>
    public static class JwtService
    {
        private static IConfigurationSection JwtSection
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var config = builder.Build();
                return config.GetSection("JwtSettings");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string Issuer => JwtSection.GetSection("Issuer").Value;
        /// <summary>
        /// 
        /// </summary>
        public static string Audience => JwtSection.GetSection("Audience").Value;
        /// <summary>
        /// 
        /// </summary>
        public static string SecretKey => JwtSection.GetSection("SecretKey").Value;

        /// <summary>
        /// 注册Jwt服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddJwtService(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    //认证middleware配置
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    //主要是jwt  token参数设置
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Token颁发机构
                        ValidIssuer = Issuer,
                        //颁发给谁
                        ValidAudience = Audience,
                        //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                        ValidateIssuerSigningKey = true,
                        //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true,
                        //允许的服务器时间偏移量
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
