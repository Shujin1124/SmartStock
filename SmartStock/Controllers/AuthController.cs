using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Accounts.Command;
using SmartStock.Application.Profile.Command;
using SmartStock.Application.Profile.DTO_s;
using System.Security.Claims;

namespace SmartStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result); // success → JSON
            }
            catch (UnauthorizedAccessException ex)
            {
                // return JSON error, not HTML
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // fallback for unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response); ;
        }


        [HttpGet("verify")]
        public async Task<IActionResult> Verify([FromQuery] string token)
        {
            var result = await _mediator.Send(new VerifyAccountCommand(token));
            return Ok(result);
        }
        private int GetAccountId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claim == null)
                throw new UnauthorizedAccessException("Invalid token");

            return int.Parse(claim); // ✅ no FormatException now
        }

        [HttpGet("me/profile")]
        public async Task<ActionResult<ProfileDto>> GetMyProfile()
        {
            var id = GetAccountId();
            var profile = await _mediator.Send(new GetMyProfileQuery(id));
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPut("Update/profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
        {
            var id = GetAccountId();
            await _mediator.Send(new UpdateMyProfileCommand(id, dto));
            return NoContent();
        }
    }
}
//hello world