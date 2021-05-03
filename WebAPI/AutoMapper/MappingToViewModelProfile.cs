using AutoMapper;
using Domain.Core.Models;
using Domain.Enums;
using Domain.Models;
using System.Linq;
using WebAPI.ViewModels;
using WebAPI.ViewModels.Common;

namespace WebAPI.AutoMapper
{
    public class MappingToViewModelProfile : Profile
    {
        public MappingToViewModelProfile()
        {
            #region Common
            CreateMap(typeof(Entity<>), typeof(EntityViewModel));
            #endregion

            #region User
            CreateMap<User, UserDisplayViewModel>();
            #endregion

            #region UserPermission
            CreateMap<UserPermission, PermissionEnum>()
                .ConvertUsing(m => m.Permission);
            #endregion
        }
    }
}
