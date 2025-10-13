using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Contracts;
using IdentityService.DtoModels;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthetnticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        public AuthetnticationController(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Regiter([FromBody] DtoUserRegister userRegister)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(userRegister.Email);
                var result = await _userService.Register(userRegister);
                 Response.Cookies.Append("tasty", result.Token, new CookieOptions
                 {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(12)
                });
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Такой пользователь уже существует");
            }
          
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]DtoUserLoginRequest dtoUserLoginRequest)
        {
            var token = await _userService.GetUserByEmail(dtoUserLoginRequest);
               Response.Cookies.Append("tasty", token.Token, new CookieOptions
                 {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(12)
                });
            return Ok(token);
        }
    }
}