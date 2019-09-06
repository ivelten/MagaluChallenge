using Magalu.Challenge.Application.Models.User;
using Magalu.Challenge.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public UserController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticatedUserModel>> Authenticate([FromBody] AuthenticateUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await authenticationService.AuthenticateAsync(model.Username, model.Password);

            if (token == null)
                return BadRequest(new { Message = "Invalid username and/or password." });

            return new AuthenticatedUserModel
            {
                Username = model.Username,
                JwtToken = token
            };
        }
    }
}
