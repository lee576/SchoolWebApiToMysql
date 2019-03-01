using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SchoolWebApi.SwaggerExtension
{
    /// <summary>
    /// 自定义路由 分为带版本和通用的路由
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {

        /// <summary>
        /// 分组名称,是来实现接口 IApiDescriptionGroupNameProvider
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 不带版本通用的路由
        /// </summary>
        /// <param name="controllerName"></param>
        public CustomRouteAttribute(string controllerName = "[controller]") : base($"/api/{controllerName}")
        {

        }
    }
}
