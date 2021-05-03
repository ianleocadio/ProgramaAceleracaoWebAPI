using AutoMapper;
using Domain.Core.Models;
using Domain.Transfers;
using Domain.Transfers.Common;
using WebAPI.ViewModels;

namespace WebAPI.AutoMapper
{
    public class MappingToDomainProfile : Profile
    {
        public MappingToDomainProfile()
        {
            #region Common
            CreateMap(typeof(EntityTransfer), typeof(Entity<>));
            #endregion

            #region Auth
            CreateMap<AuthViewModel, AuthTransfer>();
            CreateMap<AuthChangePasswordViewModel, AuthChangePasswordTransfer>();
            #endregion

            #region User
            CreateMap<UserCreateViewModel, UserCreateTransfer>();
            CreateMap<UserUpdateViewModel, UserUpdateTransfer>();
            #endregion

            #region UserPermission
            CreateMap<PermissionUpdateViewModel, UserPermissionUpdateTransfer>();
            #endregion
        }
    }
}
