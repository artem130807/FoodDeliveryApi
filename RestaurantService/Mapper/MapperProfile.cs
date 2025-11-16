using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantService.Dto.DtoRestaurant;
using RestaurantService.Models;

namespace RestaurantService.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Restaurant, DtoRestaurantInfo>();
            CreateMap<DtoCreateRestaurant, Restaurant>();
            CreateMap<DtoUpdateRestaurant, Restaurant>();
            CreateMap<Restaurant, DtoRestaurantRatingInfo>();
        }
    }
}