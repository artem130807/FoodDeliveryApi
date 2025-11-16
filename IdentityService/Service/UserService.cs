using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Contracts;
using IdentityService.DtoModels;
using IdentityService.Models;
using IdentityService.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using AutoMapper;
using IdentityService.Provider;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using IdentityService.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
namespace IdentityService.Service
{
    public class UserService : IUserService
    {
        private readonly IdentityDbContext _identityDbContext;
        private readonly IUserRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        private readonly IDnsEmailValidator _emailValidator;
        private readonly IMemoryCache _cache;
        private readonly IPasswordValidatorService _passwordValidator;
        private readonly ICityValidationService _cityValidate;
        public UserService(IUserRepository usersRepository, IPasswordHasher passwordHasher, IMapper mapper, IJwtProvider jwtProvider, IdentityDbContext identityDbContext, IDnsEmailValidator emailValidator, IMemoryCache cache, IPasswordValidatorService passwordValidator, ICityValidationService cityValidate)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _identityDbContext = identityDbContext;
            _emailValidator = emailValidator;
            _cache = cache;
            _passwordValidator = passwordValidator;
            _cityValidate = cityValidate;
        }

    
        public async Task<AuthDto> Login(DtoUserLoginRequest dtoUserLoginRequest)
        {
            var user = await _usersRepository.GetUserByEmail(dtoUserLoginRequest.Email);
            var result = _passwordHasher.Verify(dtoUserLoginRequest.PasswordHash, user.PasswordHash);
            if (result == false || user == null)
            {
                throw new InvalidOperationException("Пользователя не существует");
            }
            var token = _jwtProvider.GenerateToken(user);
            return new AuthDto { Token = token, Message = "Вы успешно зашли в аккаунт" };
        }

        public async Task<AuthDto> Register(DtoUserRegister dtoUserRegister)
        {
             var valid = await _cityValidate.ValidCityAsync(dtoUserRegister.City);
             if (!valid.IsValid)
             {
               throw new InvalidOperationException(valid.Message);
             }

            var user = await _usersRepository.GetUserByEmail(dtoUserRegister.Email);
            if (user != null)
                throw new InvalidOperationException("Пользователь с таким email уже существует");
                
            var passwordValid = await _passwordValidator.ValidatePasswordAsync(dtoUserRegister.PasswordHash);
            if (!passwordValid.IsValid)
                throw new InvalidOperationException(passwordValid.Message);

            var email = await _emailValidator.ValidateEmailAsync(dtoUserRegister.Email);
            if (!email.IsValid)
                throw new InvalidOperationException(email.Message);
           
            var cacheKey = $"email_verified_{dtoUserRegister.Email}"; 
            if (!_cache.TryGetValue(cacheKey, out bool isVerified) || !isVerified)
                throw new InvalidOperationException("Email не подтвержден. Сначала подтвердите email.");
                     

            var password = _passwordHasher.Generate(dtoUserRegister.PasswordHash);
            var role = await _identityDbContext.Roles.SingleOrDefaultAsync(x => x.Id == (int)RolesEnum.User) ?? throw new InvalidOperationException();
            var newuser = new Users
            {
                Id = Guid.NewGuid(),
                Name = dtoUserRegister.Name,
                Email = dtoUserRegister.Email,
                PasswordHash = password,
                City = dtoUserRegister.City,
                Roles = [role]
            };
            await _usersRepository.Register(newuser);
            var token = _jwtProvider.GenerateToken(newuser);
            _cache.Remove(cacheKey);
            return new AuthDto { Token = token, Message = "Вы успешно зарегистрировались" };
        }

        public async Task<string> UpdateCity(Guid Id, string City)
        {
             var valid = await _cityValidate.ValidCityAsync(City);
            if (!valid.IsValid)
            {
                throw new InvalidOperationException(valid.Message);
            }
            var updateCity = await _usersRepository.UpdateCity(Id, City);       
            if(updateCity == null)
            {
                throw new InvalidOperationException("Ошибка");
            }
            return updateCity;
        }

        public async Task UpdatePasswordHash(string email, string password)
        {
            var user = await _usersRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("Пользователя с таким email не существует");
            }
            var passwordValid = await _passwordValidator.ValidatePasswordAsync(password);
            if (!passwordValid.IsValid)
                throw new InvalidOperationException(passwordValid.Message);
            var emailValid = await _emailValidator.ValidateEmailAsync(email);
            if (!emailValid.IsValid)
                throw new InvalidOperationException(emailValid.Message); 

            var cacheKey = $"email_verified_{email}";
            if (!_cache.TryGetValue(cacheKey, out bool isVerified) || !isVerified)
                throw new InvalidOperationException("Email не подтвержден. Сначала подтвердите email для смены пароля.");
                
            string passwordHash = _passwordHasher.Generate(password);
            await _usersRepository.UpdatePasswordHash(email, passwordHash);
        }
    }
}