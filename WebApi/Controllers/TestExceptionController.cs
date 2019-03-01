using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// Exceptionless 异常框架测试
    /// </summary>
    [Route("api/[controller]")]
    public class TestExceptionController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SendException()
        {
            try
            {
                throw new Exception("SendException 的异常");
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                //log.Error("错误:" + ex);
            }
            return Ok();
        }
    }
}
