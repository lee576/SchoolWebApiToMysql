using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;
using Swashbuckle.AspNetCore.Swagger;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class T4Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GeneritorSwaggerHtml(string templatePath, SwaggerDocument model)
        {
            var template = System.IO.File.ReadAllText(templatePath);
            var result = Engine.Razor.RunCompile(template, "Titan", typeof(SwaggerDocument), model);
            return result;
        }
    }
}
