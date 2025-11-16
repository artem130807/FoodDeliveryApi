using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Filters;
using RestaurantService.Models;
using RestaurantService.Page;

namespace RestaurantService
{
    public static class Extensions
    {
        public static void AddDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RestaurantServiceDbContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
        public static IQueryable<Restaurant> Filter(this IQueryable<Restaurant> query, RestaurantFilter restaurantFilter)
        {
            if (!string.IsNullOrEmpty(restaurantFilter.City))
                query = query.Where(x => x.City == restaurantFilter.City);

            if (!string.IsNullOrEmpty(restaurantFilter.CuisineType))
                query = query.Where(x => x.CuisineType == restaurantFilter.CuisineType);

            if (restaurantFilter.Rating.HasValue)
                query = query.Where(x => x.Rating >= restaurantFilter.Rating);

            if (restaurantFilter.DeliveryFee.HasValue)
                query = query.Where(x => x.DeliveryFee <= restaurantFilter.DeliveryFee);

            if (restaurantFilter.DeliveryTime.HasValue)
                query = query.Where(x => x.DeliveryTime <= restaurantFilter.DeliveryTime);

            return query;
        } 
        public static async Task<PagedResult<Restaurant>> ToPagedAsync(this IQueryable<Restaurant> query, PageParams pageParams)
        {
            var count = await query.CountAsync();
            if (count == 0) return new PagedResult<Restaurant>([], 0);
            var page = pageParams.Page ?? 1;
            var PageSize = pageParams.PageSize ?? 10;
            var skip = (page - 1) * PageSize;
            var result = await query.Skip(skip).Take(PageSize).ToListAsync();
            return new PagedResult<Restaurant>(result, count); 
        }
    }
}