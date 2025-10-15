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
namespace IdentityService.Service
{
    public class UserService : IUserService
    {
        private readonly IdentityDbContext _identityDbContext;
        private readonly IUserRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        public UserService(IUserRepository usersRepository, IPasswordHasher passwordHasher, IMapper mapper, IJwtProvider jwtProvider, IdentityDbContext identityDbContext)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _identityDbContext = identityDbContext;
        }
        public async Task<AuthDto> GetUserByEmail(DtoUserLoginRequest dtoUserLoginRequest)
        {
            var user = await _usersRepository.GetUserByEmail(dtoUserLoginRequest.Email);
            var result = _passwordHasher.Verify(dtoUserLoginRequest.PasswordHash, user.PasswordHash);
            if (result == false)
            {
                throw new InvalidOperationException("Пользователя не существует");
            }
            var token = _jwtProvider.GenerateToken(user);
            return new AuthDto { Token = token, Message = "Вы успешно зашли в аккаунт" };
        }

        public async Task<AuthDto> Register(DtoUserRegister dtoUserRegister)
        {
            var password = _passwordHasher.Generate(dtoUserRegister.PasswordHash);
            var role = await _identityDbContext.Roles.SingleOrDefaultAsync(x => x.Id == (int)RolesEnum.User) ?? throw new InvalidOperationException();
            var newuser = new Users
            {
                Id = Guid.NewGuid(),
                Name = dtoUserRegister.Name,
                Email = dtoUserRegister.Email,
                PasswordHash = password,
                Roles = [role]
            };
            await _usersRepository.Register(newuser);
            var token = _jwtProvider.GenerateToken(newuser);
            return new AuthDto { Token = token, Message = "Вы успешно зарегистрировались" };
        }
    }
}