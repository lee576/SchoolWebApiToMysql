using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 学校管理成
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class SchoolInfoController : Controller
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_school_InfoService _tb_school_InfoService;
        private Itb_school_departmentService _tb_school_department;
        private Itb_userinfoService _tb_userinfoService;
        public SchoolInfoController(Itb_school_InfoService tb_school_InfoService,
            Itb_school_departmentService tb_school_department,
            Itb_userinfoService tb_userinfoService)
        {
            _tb_school_InfoService = tb_school_InfoService;
            _tb_school_department = tb_school_department;
            _tb_userinfoService = tb_userinfoService;
        }
        /// <summary>
        /// 添加学校信息
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="School_name"></param>
        /// <param name="province"></param>
        /// <param name="type"></param>
        /// <param name="pid"></param>
        /// <param name="appid"></param>
        /// <param name="private_key"></param>
        /// <param name="publicKey"></param>
        /// <param name="alipay_public_key"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddSchoolInfo(string schoolcode,string School_name,string province,string pid,string appid,string private_key, string publicKey,string alipay_public_key)
        {
            try
            {
                tb_school_info model = new tb_school_info();
                if (_tb_school_InfoService.Any(x=>x.School_Code == schoolcode))
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = "schoolcode已存在",
                    });
                }
                model.School_Code = schoolcode;
                model.alipay_public_key = alipay_public_key;
                model.app_id = appid;
                model.batch = 0;
                model.pid = pid;
                model.private_key = private_key;
                model.project_no = schoolcode + "000";
                model.province = province;
                model.publicKey = publicKey;
                model.School_name = School_name;
                model.type = 0;
                model.xiyunMCode = null;
                _tb_school_InfoService.Insert(model);
                tb_userinfo usermodel = new tb_userinfo();
                usermodel.dining_talls = null;
                usermodel.loginuser = "admin";
                usermodel.menus = null;
                usermodel.password = "B9925A53B14D40D64AFFD976F3F7601F";
                usermodel.receivables = null;
                usermodel.remark = null;
                usermodel.roletype = null;
                usermodel.schoolcode = schoolcode;
                usermodel.userName = "admin";
                _tb_userinfoService.Insert(usermodel);
                tb_school_department department = new tb_school_department();
                department.isType = null;
                department.name = School_name;
                department.p_id = 0;
                department.schoolcode = schoolcode;
                department.treeLevel = 0;
                _tb_school_department.Insert(department);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess,
                });
            }
            catch (Exception ex)
            {
                Log.Error("错误" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.AddFail,
                });
            }
        }
    }
}
