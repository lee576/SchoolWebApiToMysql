using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 应用程序生命周期
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class LifetimeController : Controller
    {
        private IApplicationLifetime _applicationLifetime;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationLifetime"></param>
        public LifetimeController(IApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }

        /// <summary>
        /// 停止后台API服务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StopApi()
        {
            _applicationLifetime.StopApplication();
            return View();
        }
    }
}