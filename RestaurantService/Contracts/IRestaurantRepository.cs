using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantService.Filters;
using RestaurantService.Models;
using RestaurantService.Page;

namespace RestaurantService.Contracts
{
    public interface IRestaurantRepository
    {
        Task<PagedResult<Restaurant>> GetRestaurants(PageParams pageParams);
        Task<Restaurant> GetRestaurantById(Guid Id);
        Task<PagedResult<Restaurant>> GetRestaurantIsOpenAsync(PageParams pageParams);
        Task<PagedResult<Restaurant>> GetActiveRestaurants(PageParams pageParams);
        Task AddRestaurant(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurant(Guid Id ,Restaurant restaurant);
        Task DeleteRestaurant(Guid Id);
        Task<Restaurant> GetRestaurantByName(string RestaurantName);
        Task UpdateRatingRestaurant(Guid Id, decimal rating);
        Task <PagedResult<Restaurant>> GetRestaurantsByFilter(RestaurantFilter restaurantFilter, PageParams pageParams);
    }
}