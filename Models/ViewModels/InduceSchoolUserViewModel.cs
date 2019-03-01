using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class InduceSchoolUserViewModel
    {
        public DateTime? create_time { get; set; }
        public string department { get; set; }
        public string student_id { get; set; }
        public string user_name { get; set; }
        public string passport { get; set; }
        public string card_state { get; set; }
        public string class_id { get; set; }

        public byte? welcome_flg { get; set; }

        public string welcome_flgName { get; set; }
    }
}
