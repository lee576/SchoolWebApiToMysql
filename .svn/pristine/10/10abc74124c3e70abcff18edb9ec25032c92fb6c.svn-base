using DbModel;
using IService;
using Infrastructure.Service;
using Infrastructure;
using SqlSugar;
using Models.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace Service
{
    public class tb_school_card_template_formerService : GenericService<tb_school_card_template_former>,Itb_school_card_template_formerService
    {
        public List<CardTemplateformer> GetSchoolCardTempFormer(string schoolcode)
        {
           

                string sql = @"select e.*,c.alipay_url background_url from (select a.*,b.alipay_url from tb_school_card_template_former a left JOIN 
tb_alipay_image b on a.Logo_id=b.alipay_id) e LEFT JOIN tb_alipay_image c on e.background_id=c.alipay_id";

                using (var db = DbFactory.GetSqlSugarClient())
                {
                    return db.Ado.SqlQuery<CardTemplateformer>(sql);
                }


               // return db.Queryable<tb_school_card_template_former, tb_alipay_image>(
               //     (pi, pr) =>
               //     new object[] {
               //         JoinType.Inner,pi.Logo_id == pr.alipay_id
               //     }
               // )
               // .Where((pi, pr) => pi.schoolid==schoolcode)
               // .Select((pi, pr) => new
               // {
               //     id = pi.id,
               //     Logo_id = pi.Logo_id,
               //     schoolid = pi.schoolid,
               //     T_card_action_list = pi.T_card_action_list,
               //     T_column_info_list = pi.T_column_info_list,
               //     imgurl = pr.alipay_url
               // })
               //.ToList();

            
        }
    }
}