﻿using DbModel;
using Infrastructure.Service;
using Models.ViewModels;
using System.Collections.Generic;

namespace IService
{
    public interface Itb_school_card_templateService : IServiceBase<tb_school_card_template>
    {
         List<SchoolCardList> GetSchoolCardList(string schoolid);

        List<StudentSchoolCard> GetStudentSchoolCard(string cert_no, string schoolid);
        List<CardTypes> GetCardListtype(string schoolid);
    }
}