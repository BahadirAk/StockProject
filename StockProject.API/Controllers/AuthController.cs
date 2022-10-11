using Microsoft.AspNetCore.Mvc;
using StockProject.Business.Interfaces;
using StockProject.Dtos.AuthDtos;

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
        public IActionResult Login(LoginDto dto)
        {
            var user = _authService.Login(dto);
            return user.Result.ResponseType == Common.ResponseType.Success ? Created("", new JwtTokenGenerator().GenerateToken(user.Result.Data)) : Ok(user.Result.Message);
        }
    }
}
