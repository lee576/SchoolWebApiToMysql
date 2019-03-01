using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModels
{
    public class view_entrance_Record
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public string student_id { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string user_realname { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string device_id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string device_name { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string school_name { get; set; }

        /// <summary>
        /// 门禁时间
        /// </summary>
        public DateTime? open_time { get; set; }

        /// <summary>
        /// 门禁状态
        /// </summary>
        public string entrance_status { get; set; }


    }
}
