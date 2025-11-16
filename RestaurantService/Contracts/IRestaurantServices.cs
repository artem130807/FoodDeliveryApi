using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantService.Dto.DtoRestaurant;
using RestaurantService.Filters;
using RestaurantService.Models;
using RestaurantService.Page;

namespace RestaurantService.Contracts
{
    public interface IRestaurantServices
    {
        Task<PagedResult<DtoRestaurantInfo>> GetRestaurants(PageParams pageParams);
        Task<DtoRestaurantInfo> GetRestaurantById(Guid Id);
        Task<PagedResult<DtoRestaurantInfo>> GetActiveRestaurants(PageParams pageParams);
        Task<DtoRestaurantInfo> GetRestaurantByName(string RestaurantName);  
        Task AddRestaurant(DtoCreateRestaurant dtoCreateRestaurant);
        Task<DtoRestaurantInfo> UpdateRestaurant(Guid Id, DtoUpdateRestaurant dtoUpdateRestaurant);
        Task<PagedResult<DtoRestaurantInfo>> GetRestaurantIsOpen(PageParams pageParams);
        Task DeleteRestaurant(Guid Id);
        Task UpdateRatingRestaurant(Guid Id, decimal rating);
        Task <PagedResult<DtoRestaurantInfo>> GetRestaurantsByFilter(RestaurantFilter restaurantFilter, PageParams pageParams);
    }
}