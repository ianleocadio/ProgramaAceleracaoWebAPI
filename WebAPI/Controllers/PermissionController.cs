using AutoMapper;
using Domain.Enums;
using Domain.Transfers;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PermissionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserPermissionService _userPermissionService;

        public PermissionController(IMapper mapper, IUserPermissionService userPermissionService)
        {
            this._mapper = mapper;
            this._userPermissionService = userPermissionService;
        }

        [HttpPut]
        //[HasPermission(PermissionEnum.PermissionManage)]
        public async Task<IActionResult> Add([FromBody] IEnumerable<PermissionUpdateViewModel> request, CancellationToken cancellationToken)
        {
            var permissionUpdateTransfers = this._mapper.Map<IEnumerable<UserPermissionUpdateTransfer>>(request);

            await this._userPermissionService.AddAsync(permissionUpdateTransfers, cancellationToken);

            return Ok();
        }

        [HttpPut]
        //[HasPermission(PermissionEnum.PermissionManage)]
        public async Task<IActionResult> Remove([FromBody] IEnumerable<PermissionUpdateViewModel> request, CancellationToken cancellationToken)
        {
            var permissionUpdateTransfers = this._mapper.Map<IEnumerable<UserPermissionUpdateTransfer>>(request);

            await this._userPermissionService.RemoveAsync(permissionUpdateTransfers, cancellationToken);

            return Ok();
        }
    }
}
