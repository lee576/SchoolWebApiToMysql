using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Iview_entrance_recordService : IServiceBase<view_entrance_Record>
    {
        IEnumerable<view_entrance_Record> FindAll(string deviceId = "", string studentIdentity = "",
                    string startTime = "", string endTime = "", string schoolCode = "", string stuffType = "");

        IEnumerable<view_entrance_Record> FindAll(int pageIndex, int pageSize, ref int total,
            string deviceId = "", string studentIdentity = "",
            string startTime = "", string endTime = "", string schoolCode = "", string stuffType = "");
    }
}