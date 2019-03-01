using Infrastructure;
using Infrastructure.Service;
using IService;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class SchoolCodrService:GenericService<SchoolUser>, ISchoolCodrService
    {
        public IEnumerable<SchoolUser> GetSchoolUser(string cert_no)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = @"select b.template_id,b.card_show_name,a.student_id,a.school_id,a.card_validity,a.user_id,a.department,a.welcome_flg from tb_school_user a,tb_school_card_template b where passport='" + cert_no + "' and a.card_add_id=b.card_add_id and b.school_id =a.school_id";
                var data = db.Ado.SqlQuery<SchoolUser>(sql);
                return data;
            }
        }

        /// <summary>
        /// 返回校园卡按学校分类
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SchoolPid> SchoolType(string school_id="")
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = "";
                if (school_id=="")
                 sql = @"select school_id,pid from tb_school_card_template a left join tb_school_info b on a.school_id=b.school_code  group by school_id,pid";
                else
                    sql = @"select school_id,pid from tb_school_card_template a left join tb_school_info b on a.school_id=b.school_code  group by school_id,pid  having school_id='"+ school_id+"'";
                var data = db.Ado.SqlQuery<SchoolPid>(sql);
                return data;
            }


        }

        /// <summary>
        /// 根据ID返回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<SchoolPid> SchoolTypebyid(string id)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = "";
                    sql = @" select school_id,pid from tb_school_card_template a left join tb_school_info b on a.school_id=b.school_code  where a.id="+id;
                var data = db.Ado.SqlQuery<SchoolPid>(sql);
                return data;
            }


        }

        /// <summary>
        /// 返回校园卡种类
        /// </summary>
        /// <returns></returns>
        public IEnumerable<card_type_list> SchoolCrdType(string school_id = "")
        {

            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = "";
                if(school_id=="")
                sql = @"select card_add_id card_type_id,school_id,max(card_show_name) card_type_name from tb_school_card_template group by card_add_id,school_id";
                else
                    sql = @"select card_add_id card_type_id,school_id,max(card_show_name) card_type_name from tb_school_card_template group by card_add_id,school_id having school_id='" + school_id + "'";
                var data = db.Ado.SqlQuery<card_type_list>(sql);
                return data;
            }

        }

        /// <summary>
        /// 根据ID返回卡种
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<card_type_list> SchoolCrdTypebyid(string id)
        {

            using (var db = DbFactory.GetSqlSugarClient())
            {
                string sql = "";
                    sql = @"select card_add_id card_type_id,school_id,card_show_name card_type_name from tb_school_card_template  where id="+id;
               
                var data = db.Ado.SqlQuery<card_type_list>(sql);
                return data;
            }

        }




    }
}
