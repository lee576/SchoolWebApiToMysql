using System.Collections.Generic;
using DbModel;
using Infrastructure.Service;
using Models.ViewModels;

namespace IService
{
    public interface Itb_payment_typeService : IServiceBase<tb_payment_type>
    {
        /// <summary>
        /// 通过学校编码获取缴费类型和缴费项目信息
        /// </summary>
        /// <param name="school_code"></param>
        /// <returns></returns>
        List<view_payment_itemType> GetTypeChildList(string school_code, string passport = "");
        List<view_payment_item> GetPayItemsToid(string school_code, int id);
    }
}