﻿using AutoMapper;
using DbModel;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using SchoolWebApi.HeadParams;
using System.Linq;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 用户权限管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class UserRightMangeController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private readonly Itb_userinfoService _tb_userinfoService;
        private readonly Itb_menuinfoService _tb_menuinfoService;
        private readonly IMapper _mapper;

        private string GetSchoolCode(string schoolCode)
        {
            var headers = HttpContext.Request.Headers;
            if (headers.TryGetValue("schoolcode", out var headerValues))
            {
                schoolCode = headerValues.First();
            }
            return schoolCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public UserRightMangeController(Itb_userinfoService tb_userinfoService,
            Itb_menuinfoService tb_menuinfoService,
            IMapper mapper)
        {
            _tb_userinfoService = tb_userinfoService;
            _tb_menuinfoService = tb_menuinfoService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="userNameOrLoginuserOrRole">姓名,登录账号或角色名</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult GetUserList(int iDisplayStart, int iDisplayLength,string userNameOrLoginuserOrRole)
        {
            int pageStart = iDisplayStart;
            int pageSize = iDisplayLength;
            int pageIndex = (pageStart / pageSize) + 1;
            int totalRecordNum = default(int);
            //从请求Http Headers中获取学校编码
            var schoolCode = string.Empty;
            schoolCode = GetSchoolCode(schoolCode);
            //用户列表
            var userList = _tb_userinfoService.GetUserList(pageIndex, pageSize, ref totalRecordNum, schoolCode, userNameOrLoginuserOrRole);
            return Json(new
            {
                code = JsonReturnMsg.SuccessCode,
                msg = JsonReturnMsg.GetSuccess,
                iTotalRecords = totalRecordNum,
                iTotalDisplayRecords = totalRecordNum,
                data = userList
            });
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult AddUser([FromBody]LoginUser user)
        {
            var isExist = _tb_userinfoService.IsExistUser(user.loginuser, user.schoolcode);
            if (isExist)
            {
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = @"账户已存在",
                });
            }

            var userInfo = _mapper.Map<LoginUser, tb_userinfo>(user);

            _tb_userinfoService.Insert(userInfo);
            return Json(new
            {
                code = JsonReturnMsg.SuccessCode,
                msg = JsonReturnMsg.AddSuccess,
            });
        }

        /// <summary>
        /// 根据用户Id,学校编码,获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult GetUserById(int id)
        {
            var user = _tb_userinfoService.FindById(id);
            return Json(new
            {
                code = JsonReturnMsg.SuccessCode,
                msg = JsonReturnMsg.AddSuccess,
                aaData = user
            });
        }

        /// <summary>
        /// 根据用户Id删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult DeleteById(int id)
        {
            object[] ids = { id };
            _tb_userinfoService.DeleteByIds(ids);
            return Json(new
            {
                code = JsonReturnMsg.SuccessCode,
                msg = JsonReturnMsg.DeleteSuccess
            });
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult UpdateUser([FromBody]tb_userinfo userInfo)
        {
            var isSucess = _tb_userinfoService.Update(userInfo);
            if (isSucess)
            {
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.UpdateSuccess
                });
            }
            return Json(new
            {
                code = JsonReturnMsg.FailCode,
                msg = JsonReturnMsg.UpdateFail
            });
        }

        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult GetMenus()
        {
            var menu = _tb_menuinfoService.GetMenu();
            var menus = _tb_menuinfoService.GetMenus();
            return Json(new
            {
                code = JsonReturnMsg.SuccessCode,
                msg = JsonReturnMsg.GetSuccess,
                Data = menu,
                Datas = menus
            });
        }
    }
}