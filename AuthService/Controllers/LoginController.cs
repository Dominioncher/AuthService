using AuthService.Controllers.DTO;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthentificationService _authService;

        public LoginController(AuthentificationService service)
        {
            _authService = service;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _authService.GenerateUserJWTToken(request.Login, request.Password);
            return Ok(token);
        }
    }
}
