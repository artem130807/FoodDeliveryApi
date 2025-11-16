using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityService.Contracts;
using IdentityService.DtoModels;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthetnticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IEmailVerficationService _verificationService;
        private readonly IMemoryCache _cache;
        private readonly IDnsEmailValidator _emailValidator;
        public AuthetnticationController(IUserService userService, IUserRepository userRepository, IEmailService emailService, IEmailVerficationService verficationService, IMemoryCache cache, IDnsEmailValidator emailValidator)
        {
            _userService = userService;
            _userRepository = userRepository;
            _emailService = emailService;
            _verificationService = verficationService;
            _cache = cache;
            _emailValidator = emailValidator;
        }
        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendVerificationRequest verificationRequest)
        {
            await _verificationService.SendVerificationAsync(verificationRequest.Email);
            return Ok("Новый код подтверждения");
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequest request)
        {
            var cacheKey = $"verification_{request.Code}";
            if (!_cache.TryGetValue(cacheKey, out string email) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Код ненайден или устарел");
            }
            var isValid = await _verificationService.Verificate(request.Code, email);
            if (!isValid)
            {
                return BadRequest("Неправильный код");
            }
            _cache.Remove(cacheKey);
            return Ok("Email подтвержден");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Regiter([FromBody] DtoUserRegister userRegister)
        {
            try
            {          
            var result = await _userService.Register(userRegister);
            Response.Cookies.Append("tasty", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(12)
            });
            await _verificationService.DeleteEmailVerificate(userRegister.Email);         
            return Ok(result);
                
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }      
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DtoUserLoginRequest dtoUserLoginRequest)
        {
            try
            {
                var token = await _userService.Login(dtoUserLoginRequest);
                Response.Cookies.Append("tasty", token.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(12)
                });
                return Ok(token);
            }
            catch (Exception)
            {
                return BadRequest("Такого пользователя не существует");
            }
        }
        [Authorize]
        [HttpPatch("updatePassword")]
        public async Task<IActionResult> UpdatePassword(string email, string password)
        {
            try
            {
                await _userService.UpdatePasswordHash(email, password);
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPatch("updateCity")]
        public async Task<IActionResult> UpdateCity(string City)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue("userId"));
                var updateCity = await _userService.UpdateCity(userId, City);
                return Ok(updateCity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }

    }
}