using Magalu.Challenge.Application.Models.User;
using Magalu.Challenge.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Magalu.Challenge.Web.Api.Controllers
{
    public abstract class ApplicationController : ControllerBase
    {
        protected ActionResult GetResult(Result result)
        {
            switch (result.Type)
            {
                case ResultType.BadRequest:
                    return BadRequest(result.ErrorMessage);

                case ResultType.NotFound:
                    return NotFound(null);

                case ResultType.Success:
                    return NoContent();

                case ResultType.Forbidden:
                    return Forbid();

                default:
                    throw new ArgumentException($"Result type {result.Type} is not supported.", nameof(result));
            }
        }

        protected ActionResult<T> GetResult<T>(Result<T> result)
        {
            switch (result.Type)
            {
                case ResultType.BadRequest:
                    return BadRequest(result.ErrorMessage);

                case ResultType.NotFound:
                    return NotFound(null);

                case ResultType.Success:
                    return Ok(result.Value);

                default:
                    throw new ArgumentException($"Result type {result.Type} is not supported.", nameof(result));
            }
        }

        protected ActionResult<AuthenticatedUserModel> GetResult(AuthenticationResult result)
        {
            if (result.IsAuthenticated)
                return Ok(result.AuthenticatedUser);

            return BadRequest(result.ErrorMessage);
        }
    }
}
