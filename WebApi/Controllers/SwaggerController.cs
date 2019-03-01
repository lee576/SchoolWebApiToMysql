using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApi.Extensions;
using SchoolWebApi.SwaggerExtension;
using SchoolWebApi.Utility;
using Swashbuckle.AspNetCore.SwaggerGen;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [CustomRoute]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SwaggerController : ApiControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SpireDocHelper _spireDocHelper;
        private readonly SwaggerGenerator _swaggerGenerator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="spireDocHelper"></param>
        /// <param name="swaggerGenerator"></param>
        public SwaggerController(IHostingEnvironment hostingEnvironment, SpireDocHelper spireDocHelper, SwaggerGenerator swaggerGenerator)
        {
            _hostingEnvironment = hostingEnvironment;
            _spireDocHelper = spireDocHelper;
            _swaggerGenerator = swaggerGenerator;//从ioc容器中获取swagger生成文档的对象
        }

        /// <summary>
        /// Swagger 文档导出
        /// </summary>
        /// <param name="type"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpGet("ExportApiWord", Name = "ExportApiWord")]
        public FileResult ExportApiWord(string type, string version)
        {
            string memi = string.Empty;
            string fileExten = string.Empty;
            Stream outdata = null;

            //1.获取api文档json，version是版本，根据指定版本获取指定版本的json对象。
            var model = _swaggerGenerator.GetSwagger(version);
            if (model == null)
            {
                throw new Exception("Swagger Json cannot be equal to null！");
            }
            //2.使用微软的mvc模板引擎技术来生成html
            var html = T4Helper.GeneritorSwaggerHtml($"{_hostingEnvironment.WebRootPath}\\SwaggerDoc.cshtml", model);
            //3.将html转换成需要导出的文件类型。
            var op = _spireDocHelper.SwaggerHtmlConvers(html, type, out memi);
            if (!op.Successed)
            {
                throw new Exception(op.Message);
            }
            outdata = op.Data;
            return File(outdata, memi, $"API文档 {version}{type}");//返回文件流，type是文件格式
        }
    }
}
