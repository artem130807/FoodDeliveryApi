using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.DtoModels;
using IdentityService.Models;

namespace IdentityService.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            //Авторизация, Аутентификация
            CreateMap<Users, DtoUser>();
            CreateMap<Users, DtoUserLoginRequest>();
            CreateMap<Users, DtoUserRegister>();
            //Email проверка
            CreateMap<EmailVerification, SendVerificationRequest>();
            CreateMap<EmailVerification, VerifyCodeRequest>(); 
        }
    }
}