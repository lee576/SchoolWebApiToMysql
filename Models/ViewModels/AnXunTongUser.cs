namespace Models.ViewModels
{
    /// <summary>
    /// 安迅通智能门锁,开门用户信息,所有需加密字段采用3DES加密算法
    /// </summary>
    public class AnXunTongUser
    {
        /// <summary>
        /// 客户端账号: 需要与服务端提供的相符,不需要加密
        /// </summary>
        public string ClientAccount { set; get; }
        /// <summary>
        /// 客户端密码: 需要与服务端提供的相符，需要加密
        /// </summary>
        public string ClientPassword { set; get; }
        /// <summary>
        /// 添加修改用户时添加的房间全称，需要加密，每层级别中间连接用--。 使用的锁系统数据库中bedchamber表的Name值
        /// </summary>
        public string BedchamberNames { set; get; }
        /// <summary>
        /// 添加、修改和删除用户时使用的证件号，需要加密。唯一，修改和删除用户时需要根据证件号来获取用户
        /// </summary>
        public string CridentialId { set; get; }
        /// <summary>
        /// 开始时间: 格式为“yyyyMMddHHmm”，需要加密
        /// </summary>
        public string StartTime { set; get; }
        /// <summary>
        /// 结束时间: 格式为 “yyyyMMddHHmm”，需要加密
        /// </summary>
        public string EndTime { set; get; }
    }
}
