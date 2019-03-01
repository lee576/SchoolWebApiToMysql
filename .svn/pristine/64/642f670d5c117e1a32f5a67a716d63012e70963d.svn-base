using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class IncomingFrequency_tb_entrance_record
    {

    }
    public class xAxis
    {
        public xAxis(List<string> data)
        {
            this.type = "category";
            this.boundaryGap = false;
            this.data = data;
        }
        public string type { get; set; }
        public bool boundaryGap { get; set; }
        public List<string> data { get; set; }
    }
    public class series
    {
        public series(string name, List<string> data)
        {
            this.name = name;
            this.type = "line";
            this.stack = "总量";
            this.data = data;
        }
        public string name { get; set; }
        public string type { get; set; }
        public string stack { get; set; }
        public List<string> data { get; set; }
    }
}
