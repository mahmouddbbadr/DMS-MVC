using AutoMapper;
using DMS.Domain.Models;
using DMS.Service.ModelViews.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DMS.Service.MapperHelper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, RegisterUserViewModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.Email = src.EmailAddress;
                dest.FName = src.FirstName;
                dest.LName = src.LastName;
                dest.UserName = src.EmailAddress;
            });



        }
    }
}
