using AutoMapper;
using DMS.Domain.Models;
using DMS.Service.ModelViews.Shared;
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
            // Example mappings
            //CreateMap<AppUser, UserViewModel>();

            //Map from SharedItem → SharedItemViewModel

            CreateMap<SharedItem, SharedItemViewModel>()
               .ForMember(dest => dest.FolderName, opt => opt.MapFrom(src => src.Folder != null ? src.Folder.Name : null))
               .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.Document != null ? src.Document.Name : null))
               .ForMember(dest => dest.SharedByUserName, opt => opt.MapFrom(src => src.SharedByUser != null ? src.SharedByUser.UserName : string.Empty))
               .ForMember(dest => dest.SharedWithUserName, opt => opt.MapFrom(src => src.SharedWithUser != null ? src.SharedWithUser.UserName : string.Empty))
               .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.PermissionLevel))
               .ForMember(dest => dest.SharedDate, opt => opt.MapFrom(src => src.AddedAt))
               .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.Document.FilePath));


            //Map from ShareViewModel → SharedItem
            // Note: SharedWithUserId will be resolved manually in the service from SharedWithUserEmail

            CreateMap<ShareViewModel, SharedItem>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.SharedDate))
               .ForMember(dest => dest.PermissionLevel, opt => opt.MapFrom(src => src.Permission))
               .ForMember(dest => dest.SharedByUserId, opt => opt.MapFrom(src => src.SharedByUserId))
               .ForMember(dest => dest.SharedWithUserId, opt => opt.Ignore()) // from user by Email
               .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.DocumentId))
               .ForMember(dest => dest.FolderId, opt => opt.MapFrom(src => src.FolderId));
            
            CreateMap<SharedItem, SharedUserViewModel>()
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.SharedWithUserId))
           .ForMember(dest => dest.Email, opt => opt.Ignore()) 
           .ForMember(dest => dest.SharedDate, opt => opt.MapFrom(src => src.AddedAt));
            CreateMap<AppUser, RegisterUserViewModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.Email = src.EmailAddress;
                dest.FName = src.FirstName;
                dest.LName = src.LastName;
                dest.UserName = src.EmailAddress;
            });
            CreateMap<AppUser, UserOutputViewModel>().AfterMap((src, dest) =>
            {
                dest.EmailAddress = src.Email;
                dest.FirstName = src.FName;
                dest.LastName = src.LName;
            }).ReverseMap();



        }
    }
}
