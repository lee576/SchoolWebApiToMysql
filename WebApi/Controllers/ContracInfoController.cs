﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel;
using IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 签约管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class ContracInfoController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_ContractInfoService itb_ContractInfoService;
        private Itb_school_InfoService itb_School_InfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ContractInfoService"></param>
        /// <param name="_School_InfoService"></param>
        public ContracInfoController(Itb_ContractInfoService _ContractInfoService, Itb_school_InfoService _School_InfoService)
        {
            itb_ContractInfoService = _ContractInfoService;
            itb_School_InfoService = _School_InfoService;
        }

        #region 签约管理列表
        /// <summary>
        /// 签约管理列表
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="schoolCode_Name"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetContractInfoList(int sEcho, int iDisplayStart, int iDisplayLength, string schoolCode_Name, string startTime)
        {
            try
            {
                int pageStart = iDisplayStart;
                int pageSize = iDisplayLength;
                int pageIndex = (pageStart / pageSize) + 1;
                int totalRecordNum = default(int);
                var pageRecords = itb_ContractInfoService.FindContracInfoList(pageIndex, pageSize, ref totalRecordNum,
                    schoolCode_Name, startTime);
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    sEcho = sEcho,
                    iTotalRecords = totalRecordNum,
                    iTotalDisplayRecords = totalRecordNum,
                    aaData = pageRecords
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 新增签约者信息
        /// <summary>
        /// 新增签约者信息
        /// </summary>
        /// <param name="contractName"></param>
        /// <param name="schoolCode"></param>
        /// <param name="contractTime"></param>
        /// <param name="ali_user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AddContractInfo(string contractName, string schoolCode, DateTime contractTime, string ali_user_id)
        {
            try
            {
                tb_ContractInfo tc = new tb_ContractInfo();
                tc.contractName = contractName;
                tc.school_code = schoolCode;
                tc.ali_user_Id = ali_user_id;
                tc.contractTime = contractTime;
                tc.createTime = DateTime.Now;
                var res = itb_ContractInfoService.Insert(tc);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.AddSuccess
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 根据签约编号获取签约信息
        /// <summary>
        /// 根据签约编号获取签约信息
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetContractInfoById(int contractId)
        {
            try
            {
                var res = itb_ContractInfoService.FindById(contractId);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    aaData = res
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 修改签约者信息
        /// <summary>
        /// 修改签约者信息
        /// </summary>
        /// <param name="contractTime"></param>
        /// <param name="contractId"></param>
        /// <param name="name"></param>
        /// <param name="ali_userid"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult UpdateContractInfo(string contractTime, int contractId,string name,string ali_userid)
        {
            try
            {
                bool boolReturn = itb_ContractInfoService.UpdateColumnsByConditon(a =>
                         new tb_ContractInfo { contractTime = SqlSugar.SqlFunc.ToDate(contractTime), ali_user_Id=ali_userid, contractName=name },
                         a => a.contractId == contractId);
                if (boolReturn)
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.SuccessCode,
                        msg = JsonReturnMsg.UpdateSuccess
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = JsonReturnMsg.FailCode,
                        msg = JsonReturnMsg.UpdateFail
                    });
                }

            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 获取所有学校列表
        /// <summary>
        /// 获取所有学校列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllschoolList()
        {
            try
            {
                var list = itb_School_InfoService.FindAll();
                List<SchoolInfoViewModel> school_Infos = new List<SchoolInfoViewModel>();
                foreach (var item in list)
                {
                    SchoolInfoViewModel infoViewModel = new SchoolInfoViewModel();
                    infoViewModel.schoolName = item.School_name;
                    infoViewModel.schoolCode = item.School_Code;
                    school_Infos.Add(infoViewModel);
                }
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    aaData = school_Infos
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 获取所有签约者列表
        /// <summary>
        /// 获取所有签约者列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllContracter()
        {
            try
            {
                var list = itb_ContractInfoService.FindAll().Distinct().ToList();
                List<tb_ContractInfo> nonDuplicateList = list.Where((x, i) => list.FindIndex(z => z.ali_user_Id == x.ali_user_Id) == i).ToList();
                List<ContracterNameViewModel> c_Infos = new List<ContracterNameViewModel>();
                foreach (var item in nonDuplicateList)
                {
                    ContracterNameViewModel nameInfo = new ContracterNameViewModel();
                    nameInfo.contracterId = item.contractId;
                    nameInfo.contracterName = item.contractName;
                    c_Infos.Add(nameInfo);
                }
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    aaData = c_Infos
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 单个或批量转移签约者
        /// <summary>
        /// 单个或批量转移签约者
        /// </summary>
        /// <param name="FromcontractId"></param>
        /// <param name="TocontractId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult BatchMoveContractInfo(string FromcontractId, int TocontractId)
        {
            try
            {
                var toName = itb_ContractInfoService.FindById(TocontractId)?.contractName;

                string value = FromcontractId.Replace("\"", "").Replace("[", "").Replace("]", "");
                string[] strArr = value.Split(',');
                int[] Ids = new int[strArr.Length];
                for (int i = 0; i < strArr.Length; i++)
                { 
                    Ids[i] = Convert.ToInt32(strArr[i]);
                }
                var toContractIdInfo = itb_ContractInfoService.FindById(TocontractId);
                List<tb_ContractInfo> list = new List<tb_ContractInfo>();
                var idList = new List<string>();
                foreach (var item in Ids)
                {
                    var fromContractInfo = itb_ContractInfoService.FindById(item);
                    tb_ContractInfo info = new tb_ContractInfo();
                    info.contractName = toContractIdInfo.contractName;
                    info.school_code = fromContractInfo.school_code;
                    info.ali_user_Id = toContractIdInfo.ali_user_Id;
                    info.contractTime = fromContractInfo.contractTime;
                    list.Add(info);
                    idList.Add(item + "");
                }
                itb_ContractInfoService.DeleteByIds(idList.ToArray());
                itb_ContractInfoService.Insert(list);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = "转移成功"
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = "转移失败"
                });
            }
        }
        #endregion

        #region 根据学校编号查看学校的领卡数据
        /// <summary>
        /// 根据学校编号查看学校的领卡数据 获取单个学校流水数据（按月查询）
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult FindSchoolDetailsInfo(string sTime, string schoolCode)
        {
            try
            {
                SchoolDetailsInfoViewModel schoolDetails = new SchoolDetailsInfoViewModel();
                var schoolInfo = itb_School_InfoService.FindByClause(t => t.School_Code == schoolCode);
                string merchantCode = schoolInfo.xiyunMCode;
                var resCardInfo = itb_ContractInfoService.FindSchoolCardInfoBySchoolCode(schoolCode);
                var resXiyunNotify = itb_ContractInfoService.FindTransactionFlowingInfo(sTime, merchantCode);
                schoolDetails.schoolCardInfo = resCardInfo;
                schoolDetails.TransactionFlowing = resXiyunNotify;
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    aaData = schoolDetails
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 根据签约者编号获取所有签约的学校
        /// <summary>
        /// 根据签约者编号获取所有签约的学校
        /// </summary>
        /// <param name="ali_user_Id"></param>
        /// <param name="stime"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetContracterSchoolList(string ali_user_Id, string stime)
        {
            try
            {
                var schoolInfo = itb_ContractInfoService.FindListByClause(x => x.ali_user_Id == ali_user_Id, x => x.school_code).ToList();
                List<tb_ContractInfo> nonDuplicateList = schoolInfo.Where((x, i) => schoolInfo.FindIndex(z => z.school_code == x.school_code) == i).ToList();
                List<SchoolInfoViewModel> list = new List<SchoolInfoViewModel>();
                foreach (var item in nonDuplicateList)
                {
                    SchoolInfoViewModel info = new SchoolInfoViewModel();
                    string time = item.contractTime.Value.ToString("yyyy-MM");
                    if (time == stime)
                    {
                        var sinfo = itb_School_InfoService.FindByClause(a => a.School_Code == item.school_code);
                        info.schoolName = sinfo.School_name;
                        info.schoolCode = sinfo.School_Code.Trim();
                        list.Add(info);
                    }
                }
                return Json(new
                {
                    code = JsonReturnMsg.GetSuccess,
                    aaData = list
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

        #region 根据签约人获取学校领卡及流水总数据(按月查询)
        /// <summary>
        /// 根据签约人获取学校领卡及流水数据(按月查询)
        /// </summary>
        /// <param name="ali_user_Id"></param>
        /// <param name="stime"></param> 
        /// <returns></returns>
        [HttpGet]
        public JsonResult FindSchoolCardFlowingInfo(string ali_user_Id, string stime)
        {
            try
            {
                var list = itb_ContractInfoService.FindSchoolCardFlowingInfo(ali_user_Id, stime);
                return Json(new
                {
                    code = JsonReturnMsg.SuccessCode,
                    msg = JsonReturnMsg.GetSuccess,
                    aaData = list
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    code = JsonReturnMsg.FailCode,
                    msg = JsonReturnMsg.GetFail
                });
            }
        }
        #endregion

    }
}
