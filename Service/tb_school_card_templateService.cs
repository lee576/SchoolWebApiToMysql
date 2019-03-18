﻿using DbModel;
using IService;
using Infrastructure.Service;
using SqlSugar;
using System;
using Models.ViewModels;
using System.Text;
using Infrastructure;
using System.Collections.Generic;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api;


namespace Service
{
    public class tb_school_card_templateService : GenericService<tb_school_card_template>,Itb_school_card_templateService
    {

        /// <summary>
        /// 返回校园卡种类列表
        /// </summary>
        /// <param name="schoolid">学校编号</param>
        /// <returns></returns>
        public List<SchoolCardList> GetSchoolCardList(string schoolid)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                var clist = db.Queryable<tb_school_card_template, tb_school_user, tb_alipay_image>((tp, us, im) => new object[]{
                                        JoinType.Left,tp.School_ID==us.school_id&&tp.ID==us.card_id,
                                        JoinType.Left,tp.background_id==im.alipay_id

                                    }).Where((tp, us, im) => tp.School_ID == schoolid)
                                      .GroupBy((tp, us, im) => tp.Card_add_ID)
                                      .GroupBy((tp, us, im) => tp.card_show_name)
                                      .GroupBy((tp, us, im) => tp.background_id)
                                      .GroupBy((tp, us, im) => im.url)
                                      .Select< SchoolCardList>("tp.card_add_id,tp.card_show_name,im.alipay_url as background_url,count(us.user_id) as card_count").ToList();
                

                return clist;
            }

        }


        public List<CardTypes> GetCardListtype(string schoolid)
        {
            string sql = @"select ID,card_show_name from tb_school_card_template WHERE School_ID='" + schoolid + "' group by card_show_name";

            using (var db = DbFactory.GetSqlSugarClient())
            {



                var dr = db.Ado.SqlQuery<CardTypes>(sql);
                return dr;
            }

        }

       





        /// <summary>
        /// 通过身份证获取信息
        /// </summary>
        /// <param name="cert_no"></param>
        /// <returns></returns>
        public List<StudentSchoolCard>GetStudentSchoolCard(string cert_no,string schoolid)
        {
            string sql = @"select a.user_name,t.template_id,t.card_show_name,a.student_id,a.school_id,a.card_validity,a.user_id,a.department,a.welcome_flg from tb_school_user a INNER JOIN tb_school_card_template t
on a.card_add_id = t.Card_add_id and a.school_id = t.school_id WHERE a.passport = '" + cert_no + "' and a.school_id='"+ schoolid+"'";


                using (var db = DbFactory.GetSqlSugarClient())
            {
               

              
                var dr = db.Ado.SqlQuery<StudentSchoolCard>(sql);
                return dr;
            }

       //     using (var db = DbFactory.GetSqlSugarClient())
       //     {
       //         var clist = db.Queryable<tb_school_user, tb_school_card_template>((a, b) => new object[]{
       //               JoinType.Inner,a.school_id == b.School_ID&&b.Card_add_ID == a.card_add_id

       //         }).Where((a, b) =>a.passport == cert_no)
       //.Select<StudentSchoolCard>().ToList();


             ///   return clist;
            //}

        }


       


        

        
        
        }




}