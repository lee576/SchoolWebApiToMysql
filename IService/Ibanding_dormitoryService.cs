using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IService
{
    public interface Ibanding_dormitoryService
    {
        IEnumerable<BandingDormitory> FindUnBandingDormitory(int pageIndex, int pageSize, ref int total,
           string school_code = "", string floor_no = "", string room_no = "", string studentIdentity = "");

        IEnumerable<BandingDormitory> FindBandingDormitory(int pageIndex, int pageSize, ref int total,
           string school_code = "", string floor_no = "", string room_no = "", string studentIdentity = "");
    }
}
