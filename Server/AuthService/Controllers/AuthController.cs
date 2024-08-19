using AuthService.Data.DTOs;
using AuthService.Interfaces;
using AuthService.Services;
using AuthService.Validators;
using DataLayer.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AcademyContext _context;
        private readonly UserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(AcademyContext context, UserService userService, ITokenService tokenService)
        {
            _context = context;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("RefreshTokenTest")]
        public IActionResult RefreshTokenTest()
        {
            return Ok(_tokenService.GenerateRefreshToken());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.SignUp(request);

            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokenData = await _userService.SignIn(request.Username, request.Password);

            HttpContext.Response.Cookies.Append("tasty-cookies", tokenData.AccessToken);
            HttpContext.Response.Cookies.Append("refresh-tasty-cookies", tokenData.RefreshToken);

            return Ok();
        }

        [HttpPost("check")]
        [Authorize(Roles = "STUDENT")]
        public IActionResult Check()
        {
            return Ok("Authorized");
        }
    }
}
