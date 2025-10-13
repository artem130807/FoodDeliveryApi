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
            CreateMap<Users, DtoUser>();
            CreateMap<Users, DtoUserLoginRequest>();
            CreateMap<Users, DtoUserRegister>();
        }
    }
}