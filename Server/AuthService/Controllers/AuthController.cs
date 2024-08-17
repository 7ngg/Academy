using AuthService.Data.DTOs;
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

        public AuthController(AcademyContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
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

            var token = await _userService.SignIn(request.Username, request.Password);

            HttpContext.Response.Cookies.Append("tasty-cookies", token);

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
