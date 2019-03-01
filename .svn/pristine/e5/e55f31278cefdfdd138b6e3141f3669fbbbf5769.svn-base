using DbModel;
using Infrastructure.Service;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_ammeterService : IServiceBase<tb_ammeter>
    {
        /// <summary>
        /// 判断电表编号和房号是否重复
        /// </summary>
        /// <param name="room_id">房号</param>
        /// <param name="meterAddr">电表编号</param>
        /// <returns></returns>
        IEnumerable<tb_ammeter> CheckAmmeterInfo(string schoolcode, string meterAddr);
    }
}