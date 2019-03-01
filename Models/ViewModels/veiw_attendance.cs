using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class veiw_attendance
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>       
        public long id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string student_id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string user_name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? attendanceTime { get; set; }

        /// <summary>
        /// Desc:0签到 1签退
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? attendanceType { get; set; }
    }
}
