using DbModel;
using IService;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;
using Infrastructure;
using System.Linq;
using SqlSugar;
using System;

namespace Service
{
    public class tb_payment_typeService : GenericService<tb_payment_type>, Itb_payment_typeService
    {
        public List<view_payment_itemType> GetTypeChildList(string school_code,string passport="")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                List<view_payment_itemType> lstReturn = new List<view_payment_itemType>();
                List<tb_payment_type> lst = db.Queryable<tb_payment_type>().Where(x => x.is_display.ToString() == "1" && x.schoolcode.Equals(school_code)).ToList();
                //List<tb_payment_item> lstItem = db.Queryable<tb_payment_item>().Where(x => x.status.ToString() == "0" && x.schoolcode.Equals(school_code)).ToList();
                List<view_payment_item> lstItem = db.Queryable<tb_payment_item, tb_payment_accounts>(
                        (pi, pa) =>
                        new object[] {
                        JoinType.Left,pi.account==pa.id.ToString()
                    })
                    .Where((pi, pa) =>
                        pi.status == 0
                        && pi.schoolcode.Equals(school_code)
                        && pi.date_to > DateTime.Now
                    )
                    .Select((pi, pa) => new view_payment_item
                    {
                        id = pi.id,
                        //schoolcode = pi.schoolcode,
                        name = pi.name,
                        //is_external = pi.is_external,
                        //account = pi.account,
                        //target = pi.target,
                        //@fixed = pi.@fixed,
                        money = pi.money,
                        type = pi.type,
                        //introduction = pi.introduction,
                        //icon = pi.icon,
                        iconImg = SqlFunc.ToString(pi.icon),
                        //iconImg = "212121",
                        //group = pi.group,
                        //method = pi.method,
                        //can_set_count = pi.can_set_count,
                        //nessary_info = pi.nessary_info,
                        //date_from = pi.date_from,
                        //date_to = pi.date_to,
                        //count = pi.count,
                        //notify_link = pi.notify_link,
                        //notify_key = pi.notify_key,
                        //notify_msg = pi.notify_msg,
                        //remark = pi.remark,
                        //status = pi.status,
                        //limit = pi.limit,
                        AccountId = pa.id,
                        AccountName = pa.name,
                        appid = pa.appid,
                        class_id = SqlFunc.ToInt32(pi.class_id)
                    }).ToList();
                tb_school_user usermodel = db.Queryable<tb_school_user>().Where(x => x.school_id == school_code && x.passport == passport).ToList()[0];
                List<string> classlist = new List<string>();
                if (usermodel.department_id != null)
                {
                    classlist.Add(usermodel.department_id + "");
                }
                if (Convert.ToBoolean(usermodel.isMultiple))
                {
                    List<tb_school_user_multiple> usermultipleModel = db.Queryable<tb_school_user_multiple>().Where(x => x.school_id == school_code && x.passport == passport).ToList();
                    foreach (var item in usermultipleModel)
                    {
                        if (item.department_id != null)
                        {
                            classlist.Add(item.department_id + "");
                        }
                    }
                }
                for (int i = 0; i < lst.Count; i++)
                {
                    //view_payment_itemType model = (view_payment_itemType)lst[i];
                    view_payment_itemType model = new view_payment_itemType()
                    {
                        id = lst[i].id,
                        //schoolcode= lst[i].schoolcode,
                        name = lst[i].name,
                        //introduction = lst[i].introduction,
                        //create_time = lst[i].create_time,
                        //is_display = lst[i].is_display
                    };
                    model.lstItem = lstItem.Where(x => x.type == lst[i].id && x.class_id == 0).ToList();
                    foreach (var item in classlist)
                    {
                        List<view_payment_item> itemmodel = new List<view_payment_item>();
                        itemmodel = (lstItem.Where(x => x.type == lst[i].id && (x.class_id == Convert.ToInt32(item))).ToList());
                        model.lstItem = model.lstItem.Union(itemmodel).ToList();
                    }
                    if (model.lstItem.Count()>0)
                    {
                        lstReturn.Add(model);
                    }
                }
                return lstReturn;
            }
        }
        public List<view_payment_item> GetPayItemsToid(string school_code,int id)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                List<view_payment_item> lstItem = db.Queryable<tb_payment_item, tb_payment_accounts>(
                        (pi, pa) =>
                        new object[] {
                        JoinType.Left,pi.account==pa.id.ToString()
                    })
                    .Where((pi, pa) =>
                        pi.status == 0
                        && pi.schoolcode.Equals(school_code)
                        && pi.date_to > SqlFunc.GetDate()
                        &&pi.id == id
                    )
                    .Select((pi, pa) => new view_payment_item
                    {
                        id = pi.id,
                        name = pi.name,
                        money = pi.money,
                        type = pi.type,
                        iconImg = SqlFunc.ToString(pi.icon),
                        AccountId = pa.id,
                        AccountName = pa.name,
                        appid = pa.appid
                    }).ToList();
                return lstItem;
            }
        }
    }
}