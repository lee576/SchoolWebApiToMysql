using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IService
{
    public interface ISchoolCodrService
    {
        IEnumerable<SchoolUser> GetSchoolUser(string cert_no);

        IEnumerable<SchoolPid> SchoolType(string school_id = "");

        IEnumerable<card_type_list> SchoolCrdType(string school_id = "");

        IEnumerable<SchoolPid> SchoolTypebyid(string id);

        IEnumerable<card_type_list> SchoolCrdTypebyid(string id);
    }
}
