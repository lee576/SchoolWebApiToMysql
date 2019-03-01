using DbModel;
using IService;
using Infrastructure.Service;
using Infrastructure;
using SqlSugar;

namespace Service
{
    public class tb_school_card_template_columnService : GenericService<tb_school_card_template_column>,Itb_school_card_template_columnService
    {
        public object GetSchoolCardColumn(string schoolcode)
        {
            using (var db = DbFactory.GetSqlSugarClient())
            {
                return db.Queryable<tb_school_card_template_column, tb_alipay_image>(
                    (pi, pr) =>
                    new object[] {
                        JoinType.Inner,pi.ColumId == pr.alipay_id
                    }
                )
                .Where((pi, pr) => pi.School_ID == schoolcode)
                .Select((pi, pr) => new
                {
                    ColumId = pi.ColumId,
                    T_column_info = pi.T_column_info,
                    School_ID = pi.School_ID,
                    imageURL = pr.alipay_url
                })
               .ToList();

            }
        }
    }
}