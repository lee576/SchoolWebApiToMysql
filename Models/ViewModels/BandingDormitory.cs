using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class BandingDormitory
    {
        public int id { set; get; }
        public string user_id { set; get; }
        public string student_id { set; get; }
        public string user_name { set; get; }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string floor_name { set; get; }
        /// <summary>
        /// 房间名称
        /// </summary>
        public string room_name { set; get; }
        /// <summary>
        /// 门禁编码
        /// </summary>
        public string access_code { set; get; }
    }
}
