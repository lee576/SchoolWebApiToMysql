﻿using DbModel;
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
        NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public List<view_payment_itemType> GetTypeChildList(string school_code,string passport="")
        {
            try
            {
                using (var db = DbFactory.GetSqlSugarClient())
                {
                    List<view_payment_itemType> lstReturn = new List<view_payment_itemType>();
                    List<tb_payment_type> lst = db.Queryable<tb_payment_type>().Where(x => x.is_display.ToString() == "1" && x.schoolcode.Equals(school_code)).ToList();
                    List<view_payment_item> lstItem = db.Queryable<tb_payment_item, tb_payment_accounts>(
                            (pi, pa) =>
                            new object[] {
                        JoinType.Left,pi.account==pa.id.ToString(),
                        })
                        .Where((pi, pa) =>
                            pi.status == 0
                            && pi.schoolcode.Equals(school_code)
                            && pi.date_to >= SqlFunc.GetDate()
                            && pi.date_from <= SqlFunc.GetDate()
                        )
                        .Select((pi, pa) => new view_payment_item
                        {
                            id = pi.id,
                            name = pi.name,
                            money = pi.money,
                            type = pi.type,
                            iconImg = SqlFunc.ToString(pi.icon),
                            limit = pi.limit,
                            AccountId = pa.id,
                            AccountName = pa.name,
                            appid = pa.appid,
                            class_id = SqlFunc.ToInt32(pi.class_id),
                            level = SqlFunc.ToInt32(pi.level),
                            group = pi.group,
                            target = pi.target
                        }).ToList();
                    //-------------------------我是切割线-----------------------
                    //不显示缴纳次数
                    tb_payment_recordService prservice = new tb_payment_recordService();
                    List<view_payment_item> lstItem2 = new List<view_payment_item>();
                    foreach (var item in lstItem)
                    {
                        var datarecord = prservice.FindListByClause(x => x.payment_id == item.id && x.passport == passport&&x.status==1, t => t.id, OrderByType.Asc).ToList();
                        if (datarecord.Count() < item.limit)
                        {
                            lstItem2.Add(item);
                        }
                    }
                    if (lstItem2.Count()==0)
                    {
                        return null;
                    }
                    //-------------------------我是切割线-----------------------
                    //找出当前用户班级信息，可能是多班级
                    tb_school_departmentService dstree = new tb_school_departmentService();
                    tb_school_classinfoService cs = new tb_school_classinfoService();
                    var csModel = cs.FindListByClause(x => x.schoolcode == school_code, t => t.ID, OrderByType.Asc);
                    tb_school_user usermodel = db.Queryable<tb_school_user>().Where(x => x.school_id == school_code && x.passport == passport).ToList()[0];
                    List<tb_school_classinfo> classlist = new List<tb_school_classinfo>();
                    if (usermodel.department_id != null)
                    {
                        
                        classlist.Add(csModel.Where(x => x.ID == usermodel.department_id).ToList()[0]);
                    }
                    if (Convert.ToBoolean(usermodel.isMultiple))
                    {
                        List<tb_school_user_multiple> usermultipleModel = db.Queryable<tb_school_user_multiple>().Where(x => x.school_id == school_code && x.passport == passport).ToList();
                        foreach (var item in usermultipleModel)
                        {
                            if (item.department_id != null)
                            {
                                classlist.Add(csModel.Where(x => x.ID == item.department_id).ToList()[0]);
                            }
                        }
                    }
                    tb_school_card_templateService template = new tb_school_card_templateService();
                    var templateList = template.FindListByClause(x => x.School_ID == school_code, t => t.ID, OrderByType.Asc);
                   
                    for (int i = 0; i < lst.Count; i++)
                    {
                        view_payment_itemType model = new view_payment_itemType()
                        {
                            id = lst[i].id,
                            name = lst[i].name,
                        };
                        //所有用户
                        model.lstItem = lstItem2.Where(x => x.type == lst[i].id && x.group == 0).ToList();
                        //所有学生用户
                        model.lstItem = model.lstItem.Union(lstItem2.Where(x => x.type == lst[i].id && x.group == 1)).ToList();
                        //指定老师用户
                        if (templateList.Where(x=>x.ID == usermodel.card_id).ToList()[0].card_show_name=="教师卡")
                        {
                            model.lstItem = model.lstItem.Union(lstItem2.Where(x => x.type == lst[i].id && x.group == 3)).ToList();
                        }
                        //指定其他卡用户
                        var card_show_name = templateList.Where(x => x.ID == usermodel.card_id).ToList()[0].card_show_name;
                        if (card_show_name != "教师卡"&& card_show_name != "学生卡")
                        {
                            model.lstItem = model.lstItem.Union(lstItem2.Where(x => x.type == lst[i].id && x.group == 4)).ToList();
                        }
                        //指定班级
                        var lstItem3 = lstItem2.Where(x => x.type == lst[i].id && x.group == 2).ToList();
                        if (lstItem3.Count() != 0 && lstItem3[0].group == 2) 
                        {
                            List<view_payment_item> lstItem4 = new List<view_payment_item>();
                            foreach (var item in lstItem3)
                            {
                                if (item.level == 1 && classlist.Any(x => x.BranchID == item.class_id))
                                {
                                    lstItem4.Add(item);
                                }
                                if (item.level == 2 && classlist.Any(x => x.DepartmentID == item.class_id))
                                {
                                    lstItem4.Add(item);
                                }
                                if (item.level == 3 && item.class_id == 0)
                                {
                                    lstItem4.Add(item);
                                }
                                if (item.level == 3 && classlist.Any(x => x.ID == item.class_id))
                                {
                                    lstItem4.Add(item);
                                }
                            }
                            model.lstItem = model.lstItem.Union(lstItem4).ToList();
                        }
                        if (model.lstItem.Count() > 0)
                        {
                            lstReturn.Add(model);
                        }
                    }
                    return lstReturn;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
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
                        appid = pa.appid,
                        @fixed = pi.@fixed,
                        nessary_info = pi.nessary_info
                    }).ToList();
                return lstItem;
            }
        }
    }
}