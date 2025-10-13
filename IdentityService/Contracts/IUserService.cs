using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.DtoModels;
using Microsoft.AspNetCore.Identity.Data;

namespace IdentityService.Contracts
{
    public interface IUserService
    {
        Task<AuthDto> Register(DtoUserRegister dtoUserRegister);
        Task<AuthDto> GetUserByEmail(DtoUserLoginRequest dtoUserLoginRequest);
    }
}