using DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class CampusPay: tb_payment_ar
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Professional { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
