using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class UpOrderServiceModel
    {
        /// <summary>
        /// 应用 Id
        /// </summary>
        public long appId { set; get; }
        /// <summary>
        /// 学生id
        /// </summary>
        public long userId { set; get; }
        /// <summary>
        /// 学校或部门ID
        /// </summary>
        public long deptId { set; get; }
        /// <summary>
        /// 消费订单ID
        /// </summary>
        public string orderId { set; get; }
        /// <summary>
        /// 消费金额 (消费最小单位为：分)
        /// </summary>
        public double posPay { set; get; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public string machineId { set; get; }
        /// <summary>
        /// 消费时间
        /// </summary>
        public string posDataTime { set; get; }
        /// <summary>
        /// 转升换算 (用此参数可以计算升数，近似值 posPay/ posLitre)
        /// </summary>
        public float posLitr { set; get; }
        /// <summary>
        /// 回调地址 (异步通知地址http/https)
        /// </summary>
        public string notifyUrl { set; get; }
    }
}
