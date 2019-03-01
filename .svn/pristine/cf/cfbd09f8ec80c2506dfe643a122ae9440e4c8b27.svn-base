using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApi.Utility;
using IService;
using ServiceStack.Redis;
using SchoolWebApi;
using DbModel;
using Microsoft.AspNetCore.Hosting;
using SqlSugar;
using SchoolWebApi.Utility;

namespace SchoolWebApi.Controllers
{
    /// <summary>
    /// 校园码
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("any")]
    public class GenerateQRCodeController : Controller
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Itb_school_userService _tb_school_user;
        private Itb_QRCode_Base_ConfigService _tb_QRCode_Base_Config;
        private Itb_school_InfoService _tb_school_Info;
        private Itb_School_User_RoomService _tb_School_User_Room;
        private Itb_Building_Room_ConfigService _tb_Building_Room_Config;
        private IQRCode _iQRCode;
        private IHostingEnvironment host = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb_school_user"></param>
        /// <param name="tb_qrcode_base_config"></param>
        /// <param name="tb_school_info"></param>
        /// <param name="tb_school_user_room"></param>
        /// <param name="tb_building_room_config"></param>
        /// <param name="iQRCode"></param>
        /// <param name="host"></param>
        public GenerateQRCodeController(
          Itb_school_userService tb_school_user,
          Itb_QRCode_Base_ConfigService tb_qrcode_base_config,
          Itb_school_InfoService tb_school_info,
          Itb_School_User_RoomService tb_school_user_room,
          Itb_Building_Room_ConfigService tb_building_room_config,
          IQRCode iQRCode,
          IHostingEnvironment host)
        {
            _tb_school_user = tb_school_user;
            _tb_QRCode_Base_Config = tb_qrcode_base_config;
            _tb_school_Info = tb_school_info;
            _tb_School_User_Room = tb_school_user_room;
            _tb_Building_Room_Config = tb_building_room_config;
            _iQRCode = iQRCode;
            this.host = host;
        }

        /// <summary>
        /// 返回学生ID
        /// </summary>
        /// <param name="ali_user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStudentId(string ali_user_id)
        {
            try
            {
                //根据支付宝账号查询用户表
                var findUser = _tb_school_user.FindByClause(t => t.ali_user_id == ali_user_id);
                return Content(findUser.student_id);
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Content("ex");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 返回二维码图片的base64字符窜
        /// </summary>
        /// <param name="ali_user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetGRCodeGenrateImage(string ali_user_id)
        {
            try
            {
                //根据支付宝账号查询用户表
                var findUser = _tb_school_user.FindByClause(t => t.ali_user_id == ali_user_id);
                if (findUser != null)
                {
                    var qrCodeFullStr = GetQrCodeString(ali_user_id, findUser);
                    var logoPath = host.WebRootPath + @"\images\logo.jpg";
                    if (!string.IsNullOrEmpty(qrCodeFullStr))
                    {
                        var basa64Str = _iQRCode.GetQRCode(logoPath, qrCodeFullStr, 4);
                        if (!string.IsNullOrEmpty(basa64Str))
                            return Content(basa64Str);
                        return Content(string.Empty);
                    }
                    return Content(string.Empty);
                }
                return Content(string.Empty);
            }
            catch (Exception ex) when (
                ex is TypeInitializationException ||
                ex is OutOfMemoryException)
            {
                //强制回收内存
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT || 
                    Environment.OSVersion.Platform == PlatformID.Win32Windows)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                    log.Error("Force Recovery Memory:" + ex);
                }
                log.Error("GRCode Error:" + ex);
                return Content("图片生成出错");
            }
        }

        /// <summary>
        /// 异步处理返回二维码字符窜,提升服务器处理访问的吞吐量
        /// </summary>
        /// <param name="ali_user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetQRCodeImage(string ali_user_id)
        {
            try
            {
                //根据支付宝账号查询用户表
                var findUser = await Task.Run(() => { return _tb_school_user.FindByClause(t => t.ali_user_id == ali_user_id); });
                if (findUser != null)
                {
                    var qrCodeFullStr = await Task.Run(() => GetQrCodeString(ali_user_id, findUser));
                    return Json(new
                    {
                        return_code = 0,
                        return_qrCodeFullStr = qrCodeFullStr
                    });
                }
                return Json(new
                {
                    return_code = 0,
                    return_qrCodeFullStr = "0000000000"
                });
            }
            catch (Exception ex)
            {
                log.Error("错误:" + ex);
                return Json(new
                {
                    return_code = 0,
                    return_qrCodeFullStr = "0000000000"
                });
            }
        }

        private string GetQrCodeString(string ali_user_id, tb_school_user findUser)
        {
            //根据用户学校ID读取学校二维码配置
            var qrCodeConfig = _tb_QRCode_Base_Config.FindByClause(t => t.school_id == SqlFunc.ToInt64(findUser.school_id));

            //去学校表读取项目ID
            var schoolInfo = _tb_school_Info.FindByClause(t => t.School_Code == SqlFunc.ToString(findUser.school_id));

            //读取宿舍分配表
            var roomList = _tb_School_User_Room.FindListByClause(t => t.user_id == SqlFunc.ToString(findUser.user_id), t => t.id);

            //读取公共设施
            var publicBuildingList = _tb_Building_Room_Config.FindListByClause(t => t.school_id == SqlFunc.ToInt64(findUser.school_id) && t.ispublic == 1, t => t.id);

            //流水号 -固定8位
            string serialNumber;

            //加密长度-固定2位
            var encryptLength = "00";

            //加密长度余数-固定2位
            var encryptLengthRemain = "00";

            //用户密匙 -固定8位
            var key = qrCodeConfig.user_key.PadLeft(8, '0');

            //开门时限 -固定2位 (00: 表示没有时间限制 01: 表示有开始时间和结束时间限制)
            var doorLimit = qrCodeConfig.door_limit;

            //开门次数 -固定2位
            var doorOpenUseTime = qrCodeConfig.door_openusetime;

            //开门方式 -固定2位 
            //00 ：直接开门 01： 以项目编号 + 楼栋编号参数匹配来开门 02： 以设备MAC地址后六位匹配来开门
            var doorOpenStyle = "01";

            //开门时长 -固定4位
            var doorOpenTime = qrCodeConfig.door_opentime.PadLeft(4, '0');

            //扩展字段 -固定6位
            var extendField = qrCodeConfig.extend_field;

            //开始时间
            //var now = TimeHelper.GetWebTime();  //获取标准的北京时间,而非服务器时间
            //var startTime = now.ToString("yyMMddhhmmss"); 
            var now = DateTime.Now;
            var startTime = now.ToString("yyMMdd000000");

            //结束时间
            var endTime = now.ToString("yyMMdd235959");

            //楼栋或MAC地址数量 -固定2位
            var levelOrMacNum = "00";

            //项目编号-固定8位
            var projectNo = schoolInfo.project_no.PadLeft(8, '0');

            //拼接分配的房号，房间号-可以跟多个房号,每个房号固定8位
            var roomNoList = (from room in roomList
                              select room.room_no).Distinct();
            var roomNo = roomNoList.Aggregate(string.Empty, (current, room) => current + (room + "").PadLeft(8, '0'));

            //拼接公共设施
            roomNo = publicBuildingList.Aggregate(roomNo, (current, publicBuilding) => current + (publicBuilding.id + "").PadLeft(8, '0'));

            //楼栋或MAC地址数量 = 房间号数量
            levelOrMacNum = (roomNo.Length / 8).ToString("X2");

            //需要加密的部分 = 开门时限 + 开门次数 + 开门方式 + 开门时长 + 扩展字段 + 开始时间 + 结束时间
            var needEnCryptStr = doorLimit + doorOpenUseTime + doorOpenStyle + doorOpenTime + extendField + startTime + endTime;

            //开门方式不为直接开门时，加密串需要累加楼栋或MAC地址数量，项目编号，房间号等信息
            if (doorOpenStyle == "01")
            {
                needEnCryptStr = needEnCryptStr + levelOrMacNum + projectNo + roomNo;
            }

            //加密长度，是指加密字符串每8位，则加密长度为1
            encryptLength = (needEnCryptStr.Length % 8 == 0 ? needEnCryptStr.Length / 8 : needEnCryptStr.Length / 8 + 1)
                .ToString("X2");

            //加密长度余数是加密字符串最后不够8位，剩余多少位的数值的一半
            encryptLengthRemain = needEnCryptStr.Length % 8 == 0 ? "00" : ((needEnCryptStr.Length % 8) / 2).ToString("X2");

            //已加密字符串
            var cryptStr = XxteaHelper.Encrypt(needEnCryptStr, new UInt32[] { 0x00852953, 0x00543210, 0x00852953, 0x00543210 });

            //生成流水号即卡号
            var entranceObj = new tb_entrance_record();
            using (RedisClient redisClient = RedisHelper.CreateClient())
            {
                //流水号,达到最大8位数时,重新计数
                serialNumber = redisClient.Get<string>("serialNumber");
                if (serialNumber == "99999999")
                {
                    redisClient.Set("serialNumber", 0, TimeSpan.FromMinutes(30));
                }
                else
                {
                    redisClient.IncrementValueBy("serialNumber", 1);
                }

                serialNumber = redisClient.Get<string>("serialNumber");
                //加入Redis缓存
                entranceObj.user_id = int.Parse(findUser.user_id + "");
                redisClient.Set(serialNumber.PadLeft(8, '0'), entranceObj, TimeSpan.FromDays(1));
            }

            //二维码字符 = 用户数据 | 关联卡号 + 加密长度 + 加密长度余数 + 用户密匙 + 加密的字符串
            var qrCodeFullStr = ali_user_id + "|" + serialNumber.PadLeft(8, '0') + encryptLength + encryptLengthRemain + key +
                                cryptStr;
            return qrCodeFullStr;
        }
    }
}
