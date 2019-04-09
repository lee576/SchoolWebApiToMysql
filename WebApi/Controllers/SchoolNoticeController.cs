using Exceptionless.Json.Linq;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using SchoolWebApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 学校公告
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class SchoolNoticeController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_school_noticeService _tb_school_noticeService;
        public SchoolNoticeController(Itb_school_noticeService tb_school_noticeService)
        {
            _tb_school_noticeService = tb_school_noticeService;
        }
        /// <summary>
        ///  获取学校公告信息
        /// </summary>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="schoolcode"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolNotice(int iDisplayStart, int iDisplayLength, string schoolcode,string groupid="",string title = "")
        {
            try
            {
                int pageStart = iDisplayStart - 1;
                int pageSize = iDisplayLength;
                int pageIndex = pageStart;
                int totalRecordNum = 0;
                var data = _tb_school_noticeService.FindListByClause(x => x.schoolcode == schoolcode, t => t.id, SqlSugar.OrderByType.Desc);
                if (!string.IsNullOrWhiteSpace(groupid))
                {
                    data = data.Where(x => x.group == Convert.ToInt32(groupid));
                }
                if (!string.IsNullOrWhiteSpace(title))
                {
                    data = data.Where(x => x.title.Contains(title));
                }
                var data2 = data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                totalRecordNum = data.Count();
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    data = data2,
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail,
                });
            }
        }
        /// <summary>
        /// 添加或者修改公告信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddOrUpdateSchoolNotice([FromBody]JObject obj)
        {
            try
            {
                string msg = "";
                if (string.IsNullOrWhiteSpace(obj["id"]+""))
                {
                    tb_school_notice model = new tb_school_notice();
                    model.group = Convert.ToInt32(obj["group"] + "");
                    if (model.group==0)
                    {
                        var sp = (obj["class_id"] + "").Split('/');
                        model.class_id = Convert.ToInt32(sp[sp.Length - 1]);
                        model.level = sp.Length - 1;
                    }
                    model.content = obj["content"] + "";
                    model.releasetime = DateTime.Now;
                    model.schoolcode = obj["schoolcode"] + "";
                    model.title = obj["title"] + "";
                    _tb_school_noticeService.Insert(model);
                    msg = "添加成功!";
                }
                else
                {
                    var model = _tb_school_noticeService.FindById(Convert.ToInt32(obj["id"] + ""));
                    model.group = Convert.ToInt32(obj["group"] + "");
                    if (model.group == 0)
                    {
                        var sp = (obj["class_id"] + "").Split('/');
                        model.class_id = Convert.ToInt32(sp[sp.Length - 1]);
                        model.level = sp.Length - 1;
                    }
                    model.content = obj["content"] + "";
                    model.releasetime = DateTime.Now;
                    model.schoolcode = obj["schoolcode"] + "";
                    model.title = obj["title"] + "";
                    _tb_school_noticeService.Update(model);
                    msg = "修改成功!";
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = msg
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail,
                });
            }
        }
        /// <summary>
        /// 通过id获取公告信息
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolNoticeToID(string schoolcode,int id)
        {
            try
            {
                var data = _tb_school_noticeService.FindByClause(x => x.schoolcode == schoolcode && x.id == id);
                var department = "";
                var departmentName = "";
                if (data.group==0)
                {
                    var data2 = SchoolClassHelper.GetClassinfoToidAndLevel2(schoolcode, data.class_id,data.level);
                    department = data2.Item1;
                    departmentName = data2.Item2;
                }
                
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    data = data,
                    department = department,
                    departmentName = departmentName
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail,
                });
            }
        }
        /// <summary>
        /// 添加或者修改公告信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteSchoolNoticeToID([FromBody]JObject obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(obj["id"] + ""))
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.DeleteFail,
                    });
                }
                else
                {
                    var data = _tb_school_noticeService.FindById(Convert.ToInt32(obj["id"] + ""));
                    _tb_school_noticeService.Delete(data);
                }
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.DeleteSuccess
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.DeleteFail,
                });
            }
        }

    }
}
