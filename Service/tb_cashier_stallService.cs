using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Models.ViewModels;
using Infrastructure;
using SqlSugar;
namespace Service
{
    public class tb_cashier_stallService : GenericService<tb_cashier_stall>,Itb_cashier_stallService
    {

        /// <summary>
        /// 获取所在档口
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public List<Stall> GetStall(string schoolcode,string diningid)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
               

                var result = db.Queryable<tb_cashier_stall, tb_cashier_dining_hall>((a, t) =>
                        new object[]
                        {
                            JoinType.Inner, a.dining_tall == t.id
                        })
                  .Where((a, t) => t.schoolcode == schoolcode&&a.dining_tall==t.id)

                   .Select<Stall>().ToList();
                return result;
              
            }
        }



        /// <summary>
        /// 获取所在档口
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public List<tb_cashier_stall> GetStallAll(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {


                var result = db.Queryable<tb_cashier_stall, tb_cashier_dining_hall>((a, t) =>
                        new object[]
                        {
                            JoinType.Inner, a.dining_tall == t.id
                        })
                  .Where((a, t) => t.schoolcode == schoolcode)

                   .Select<tb_cashier_stall>().ToList();
                return result;

            }
        }



    }
}