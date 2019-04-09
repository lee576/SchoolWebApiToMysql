using DbModel;
using IService;
using Infrastructure.Service;
using System.Collections.Generic;
using Infrastructure;
using Models.ViewModels;
using System;
using SqlSugar;
using Models.DbModels;

namespace Service
{
    public class tb_school_noticeService: GenericService<tb_school_notice>, Itb_school_noticeService
    {
    }
}
