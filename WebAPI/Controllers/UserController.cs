using AutoMapper;
using Domain.Enums;
using Domain.Transfers;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Attributes;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            this._mapper = mapper;
            this._userService = userService;
        }

        [HttpPost]
        [HasPermission(PermissionEnum.UserCreate)]
        public async Task<ActionResult<UserDisplayViewModel>> Create([FromBody] UserCreateViewModel request, CancellationToken cancellationToken)
        {
            var newUser = this._mapper.Map<UserCreateTransfer>(request);

            var response = await this._userService.CreateAsync(newUser, cancellationToken);

            return Ok(this._mapper.Map<UserDisplayViewModel>(response));
        }

        [HttpPut]
        [HasPermission(PermissionEnum.UserUpdate)]
        public async Task<ActionResult<UserDisplayViewModel>> Update([FromBody] UserUpdateViewModel request, CancellationToken cancellationToken)
        {
            var updateUser = this._mapper.Map<UserUpdateTransfer>(request);

            var response = await this._userService.UpdateAsync(updateUser, cancellationToken);

            return Ok(this._mapper.Map<UserDisplayViewModel>(response));
        }

        [HttpGet("{id}")]
        [HasPermission(PermissionEnum.UserRead)]
        public async Task<ActionResult<UserDisplayViewModel?>> Get(long id, CancellationToken cancellationToken)
        {
            var response = await this._userService.GetByIdAsync(id, cancellationToken);

            return Ok(this._mapper.Map<UserDisplayViewModel?>(response));
        }

        [HttpGet]
        [HasPermission(PermissionEnum.UserRead)]
        public async Task<ActionResult<IEnumerable<UserDisplayViewModel>>> All(CancellationToken cancellationToken)
        {
            var response = await this._userService.GetAllAsync(cancellationToken);

            return Ok(this._mapper.Map<IEnumerable<UserDisplayViewModel>>(response));
        }

        [HttpDelete("{id}")]
        [HasPermission(PermissionEnum.UserDelete)]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            await this._userService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
