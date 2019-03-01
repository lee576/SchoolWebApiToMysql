using AunXunTongReference;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using SchoolWebApi.Extensions;
using SchoolWebApi.Utility;
using System;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Exception = System.Exception;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 安迅通智能门锁
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class AnXunTongController : Controller
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private readonly Itb_school_userService _tb_school_userService;
        private readonly string _wsdlAddress = "http://114.215.17.216:8080/locksys_school/webServices/remote?wsdl";
        /// <summary>
        /// 
        /// </summary>
        public AnXunTongController(Itb_school_userService tb_school_userService)
        {
            _tb_school_userService = tb_school_userService;
        }

        /// <summary>
        /// 生成要提交的XML参数
        /// </summary>
        /// <typeparam name="T">参数实体类型</typeparam>
        /// <param name="t">参数实例</param>
        /// <param name="methodName">要调用的WebService方法名</param>
        /// <returns></returns>
        private (string, IRemoteService) GenerateParamsXml<T>(T t, string methodName)
        {
            XElement typesNode = null;
            var declare = new XDeclaration("1.0", "UTF-8", null);
            var paramsDoc = new XDocument(
                new XElement("RESULT",
                    typesNode = new XElement("TYPES", new XAttribute("name", methodName)))
            );

            var props = t.GetType().GetProperties();
            foreach (PropertyInfo pro in props)
            {
                typesNode.Add(
                    new XElement("TYPE",
                        new XElement("ID", pro.Name),
                        new XElement("VALUE", pro.GetValue(t, null))
                    ));
            }
            var xml = paramsDoc.ToString();

            // 创建 HTTP 绑定对象
            var binding = new BasicHttpBinding();
            // 根据 WebService 的 URL 构建终端点对象
            var endpoint = new EndpointAddress($@"{_wsdlAddress}]({_wsdlAddress}");
            // 创建调用接口的工厂，注意这里泛型只能传入接口
            var factory = new ChannelFactory<IRemoteService>(binding, endpoint);
            // 从工厂获取具体的调用实例
            var callClient = factory.CreateChannel();
            return (xml, callClient);
        }

        /// <summary>
        /// 远程解锁
        /// </summary>
        /// <param name="aliUserId">阿里User Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> RemoteUnLock(string aliUserId)
        {
            try
            {
                //if (string.IsNullOrEmpty(aliUserId))
                //    throw new ArgumentNullException(nameof(aliUserId));

                //var studentInfo = _tb_school_userService.FindByClause(t => t.ali_user_id == aliUserId);
                //if (studentInfo == null)
                //{
                //    return Json(new
                //    {
                //        code = JsonReturnMsg.FailCode,
                //        msg = "不存在该用户!"
                //    });
                //}

                //提交用户数据到WebService接口
                var anXunUser = new AnXunTongUser
                {
                    ClientAccount = "Test",
                    ClientPassword = TripleDESHelper.Encrypt("123456"),
                    BedchamberNames = TripleDESHelper.Encrypt("长春测试--124"),
                    CridentialId = TripleDESHelper.Encrypt("test123"),
                    StartTime = TripleDESHelper.Encrypt(DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmm")),
                    EndTime = TripleDESHelper.Encrypt(DateTime.Now.AddDays(1).ToString("yyyyMMddHHmm"))
                };

                //生成要提交的XML参数
                var (xml, callClient) = GenerateParamsXml(anXunUser, "addRemotebyName");

                // 调用具体的方法
                var remote = new addRemotebyName1 { BedChamberId = xml };
                var result = await callClient.addRemotebyNameAsync(remote);

                //ResultType，0为失败，1为成功。如果为0，ResultInfo中会有对应的失败原因说明。如果为1，ResultInfo中会返回操作成功。ResultType和ResultInfo是不加密的
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result.@return);
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    ResultType = xmlDoc.DocumentElement.SelectSingleNode("TYPES/TYPE/ResultType").InnerText,
                    ResultInfo = xmlDoc.DocumentElement.SelectSingleNode("TYPES/TYPE/ResultInfo").InnerText
                });
            }
            catch (Exception ex)
            {
                var allErrors = ex.ExtractAllStackTrace();
                Log.Error("错误:" + allErrors);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = allErrors
                });
            }
        }
    }
}