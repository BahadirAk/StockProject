using Microsoft.AspNetCore.Mvc;
using StockProject.Business.Interfaces;
using StockProject.Dtos.AuthDtos;
using StockProject.Dtos.UserDtos;

namespace StockProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _authService.LoginAsync(dto);
            return user.ResponseType == Common.ResponseType.Success ? Created("", new JwtTokenGenerator().GenerateToken(user.Data)) : Ok(user.Message);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserCreateDto dto)
        {
            var addedUser = await _authService.RegisterAsync(dto);
            return Created(string.Empty, addedUser);
        }
    }
}
