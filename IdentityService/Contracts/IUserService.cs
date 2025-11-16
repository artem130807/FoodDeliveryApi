using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.DtoModels;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace IdentityService.Contracts
{
    public interface IUserService
    {
        Task<AuthDto> Register(DtoUserRegister dtoUserRegister);
        Task<AuthDto> Login(DtoUserLoginRequest dtoUserLoginRequest);
        Task UpdatePasswordHash(string email, string password);
        Task<string> UpdateCity(Guid Id, string City);
    }
}