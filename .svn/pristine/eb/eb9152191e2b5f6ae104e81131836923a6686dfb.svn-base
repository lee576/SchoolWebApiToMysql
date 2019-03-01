using DbModel;
using Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace IService
{
    public interface Itb_school_deviceService : IServiceBase<tb_school_device>
    {
        List<tb_school_device> GetList(int pageIndex, int pageSize, string schoolCode, string deviceCode, string deviceName, ref int intTotalRecords);
    }
}