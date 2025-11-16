using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Contracts;
using RestaurantService.Filters;
using RestaurantService.Models;
using RestaurantService.Page;

namespace RestaurantService.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        public static decimal summ;
        private readonly RestaurantServiceDbContext _context;
        public RestaurantRepository(RestaurantServiceDbContext context)
        {
            _context = context;
        }
        public async Task AddRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRestaurant(Guid Id)
        {
            await _context.Restaurants.Where(x => x.Id == Id)
            .ExecuteDeleteAsync();          
        }

        public async Task<PagedResult<Restaurant>> GetActiveRestaurants(PageParams pageParams)
        {
            return await _context.Restaurants.Where(x => x.IsActive == true).AsNoTracking().ToPagedAsync(pageParams); 
        }

        public async Task <PagedResult<Restaurant>> GetRestaurantsByFilter(RestaurantFilter restaurantFilter, PageParams pageParams)
        {
            var filteredQuery = await _context.Restaurants.Filter(restaurantFilter).ToPagedAsync(pageParams);
            return filteredQuery;
        }

        public async Task<Restaurant> GetRestaurantById(Guid Id)
        {
            var restaurant = await _context.Restaurants.FindAsync(Id);
            return restaurant;
        }      

        public async Task<Restaurant> GetRestaurantByName(string RestaurantName)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Name == RestaurantName);
            return restaurant;
        }

        public async Task<PagedResult<Restaurant>> GetRestaurants(PageParams pageParams)
        {
            var restaurants = await _context.Restaurants
            .ToPagedAsync(pageParams);
            return restaurants;
        }

        public async Task UpdateRatingRestaurant(Guid Id, decimal rating)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == Id);
            decimal result = ((restaurant.Rating * restaurant.RatingCount) + rating) / (restaurant.RatingCount + 1);
            await _context.Restaurants.Where(x => x.Id == Id)
            .ExecuteUpdateAsync
            (x =>
            x.SetProperty(r => r.Rating, result)
            .SetProperty(x => x.RatingCount, r => r.RatingCount + 1));
        }

        public async Task<Restaurant> UpdateRestaurant(Guid Id ,Restaurant restaurant)
        {
            await _context.Restaurants.Where(x => x.Id == Id)
            .ExecuteUpdateAsync(x =>
            x.SetProperty(x => x.Name, restaurant.Name)
            .SetProperty(x => x.Description, restaurant.Description)
            .SetProperty(x => x.City, restaurant.City)
            .SetProperty(x => x.Street, restaurant.Street)
            .SetProperty(x => x.HouseNumber, restaurant.HouseNumber)
            .SetProperty(x => x.Apartment, restaurant.Apartment)
            .SetProperty(x => x.Phone, restaurant.Phone)
            .SetProperty(x => x.CuisineType, restaurant.CuisineType)
            .SetProperty(x => x.ImageUrl, restaurant.ImageUrl)
            .SetProperty(x => x.IsActive, restaurant.IsActive)
            .SetProperty(x => x.Rating, restaurant.Rating)
            .SetProperty(x => x.DeliveryTime, restaurant.DeliveryTime)
            .SetProperty(x => x.DeliveryFee, restaurant.DeliveryFee)
            );
            var exists = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == Id);     
            return exists;
        }

        public async Task<PagedResult<Restaurant>> GetRestaurantIsOpenAsync(PageParams pageParams)
        {
            TimeSpan Time = DateTime.Now.TimeOfDay;
            var query = _context.Restaurants.Where(x => x.TimeClose <= Time && x.TimeOpen >= Time);
            return await query.ToPagedAsync(pageParams);
        }
    }
}