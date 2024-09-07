using AuthService.Data;
using AuthService.Data.DTOs;
using AuthService.Interfaces;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository;

        public AuthController(UserService userService, ITokenService tokenService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshTokenTest(TokenInfoDTO request)
        {
            var (v, status) = await _userService.RefreshTokenAsync(request);

            if (status.IsError)
            {
                return BadRequest(status);
            }

            return Ok(v);
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

            var (tokenData, status) = await _userService.SignIn(request.Username, request.Password);

            if (status.IsError)
            {
                return BadRequest(status);
            }

            HttpContext.Response.Cookies.Append("tasty-cookies", tokenData.AccessToken);
            HttpContext.Response.Cookies.Append("refresh-tasty-cookies", tokenData.RefreshToken);

            return Ok();
        }

        [HttpPost("check")]
        public IActionResult Check()
        {
            return Ok("Authorized");
        }
    }
}
