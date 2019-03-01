using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SchoolWebApi.SwaggerExtension
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            //Action 排序
            var paths = swaggerDoc.Paths.OrderBy(e => e.Key).ToList();
            swaggerDoc.Paths = paths.ToDictionary(e => e.Key, e => e.Value);
        }
    }
}
