using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SchoolWebApi.MiddleWare;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi;

namespace SchoolWebApi.SwaggerExtension
{
    /// <summary>
    /// Swagger服务
    /// </summary>
    public static class SwaggerService
    {
        /// <summary>
        /// 注册Swagger服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddScoped<SwaggerGenerator>();//GetSwagger获取swagger.json的核心代码在这里面，这里我们用ioc容器存储对象，后面直接调里面的获取json的方法。
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "校园后台API",
                        Version = "v1"
                    });
                //为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location); //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml");
                //第二参数 true 表示显示Controller 的注释
                c.IncludeXmlComments(xmlPath, true);
                c.OperationFilter<HttpHeaderOperation>();
                //Controller名称按字母顺序排序
                c.SwaggerGeneratorOptions.SortKeySelector = des => des.ActionDescriptor.AttributeRouteInfo.Name;
                //Controller里的Action名称按字母顺序排序
                c.DocumentFilter<CustomDocumentFilter>();
            });
        }
    }
}
