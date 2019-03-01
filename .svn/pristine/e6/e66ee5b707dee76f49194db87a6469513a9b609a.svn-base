using AutoMapper;
using DbModel;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public MappingProfile()
        {
            //此文件中添加所有的实体到实体间的映射
            CreateMap<LoginUser, tb_userinfo>();
            CreateMap<tb_school_device, dropDownListModel>()
                              .ForMember(dropdown => dropdown.id, schoolDevice =>
                              {
                                  schoolDevice.MapFrom(s => s.device_id);
                              })
                              .ForMember(dropdown => dropdown.name, schoolDevice =>
                              {
                                  schoolDevice.MapFrom(s => s.device_name);
                              });
        }
    }
}
