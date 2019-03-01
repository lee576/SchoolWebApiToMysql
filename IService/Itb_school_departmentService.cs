using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using SqlSugar;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_school_departmentService : IServiceBase<tb_school_department>
    {
        DataTable GetDepartmentInfo(string schoolcode);
        DataTable selbypid(string schoolcode);
        DataTable selbyname(string schoolcode, string name);
        int ADD(string schoolcode, string name, int pid, int level);
    }
}