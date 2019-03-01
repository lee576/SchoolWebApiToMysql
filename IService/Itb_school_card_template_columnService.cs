using DbModel;
using Infrastructure.Service;
namespace IService
{
    public interface Itb_school_card_template_columnService : IServiceBase<tb_school_card_template_column>
    {
        object GetSchoolCardColumn(string schoolcode);
    }
}