using DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class LuckDraw_PrizeModel
    {
        public string Prizeid { get; set; }
        public tb_luckdraw_prize Prize { get; set; }
        public bool isConvert { get; set; }
    }
}
