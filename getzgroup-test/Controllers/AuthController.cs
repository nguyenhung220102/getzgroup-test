using getzgroup_test.DTOs;
using getzgroup_test.Services;
using Microsoft.AspNetCore.Mvc;

namespace getzgroup_test.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        //POST /auth/sign-in
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var token = await _authService.SignInAsync(dto);
            if (token == null) return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { access_token = token, expires_in = 3600 });
        }
    }
}
