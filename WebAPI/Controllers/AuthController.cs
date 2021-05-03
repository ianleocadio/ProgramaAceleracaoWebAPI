using AutoMapper;
using Domain.Transfers;
using Microsoft.AspNetCore.Authorization;
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
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authService;

        public AuthController(IMapper mapper, IAuthenticationService authService)
        {
            this._mapper = mapper;
            this._authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] AuthViewModel request, CancellationToken cancellationToken)
        {
            var auth = this._mapper.Map<AuthTransfer>(request);

            var token = await this._authService.AuthenticateAsync(auth, cancellationToken);

            return Ok(token);
        }

        [HttpPost]
        [HasPermission]
        public async Task<ActionResult<string>> RenewToken(CancellationToken cancellationToken)
        {
            var token = await this._authService.RenewTokenAsync(cancellationToken);

            return Ok(token);
        }

        [HttpPut]
        [HasPermission]
        public async Task<IActionResult> ChangePassword([FromBody] AuthChangePasswordViewModel request, CancellationToken cancellationToken)
        {
            var authChangePassword = this._mapper.Map<AuthChangePasswordTransfer>(request);

            await this._authService.ChangePasswordAsync(authChangePassword, cancellationToken);

            return Ok();
        }
    }
}
