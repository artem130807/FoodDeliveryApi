using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using RestaurantService.Contracts;
using RestaurantService.Dto.DtoRestaurant;
using RestaurantService.Filters;
using RestaurantService.Models;
using RestaurantService.Page;
using RestaurantService.Repositories;

namespace RestaurantService.Service
{
    public class RestaurantServices : IRestaurantServices
    {
        private readonly IAddressValidationService _addressValidationService;
        private readonly IMemoryCache _cache;
        private readonly IValidationRestaurantService _validationRestaurantService;
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantStatusService _statusService;
        private readonly ICacheService _cacheService;
         private string[] prefixs =
            {
                "restaurants_filter_city",
                "restaurants_active_page",
                "restaurants_open_page",
                "restaurants_all_page",
            };
        public RestaurantServices(IMapper mapper, IRestaurantRepository restaurantRepository, IAddressValidationService addressValidationService, IValidationRestaurantService validationRestaurantService, IRestaurantStatusService statusService, IMemoryCache cache, ICacheService cacheService)
        {
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _addressValidationService = addressValidationService;
            _validationRestaurantService = validationRestaurantService;
            _statusService = statusService;
            _cache = cache;
            _cacheService = cacheService;
        }
        public async Task AddRestaurant(DtoCreateRestaurant dtoCreateRestaurant)
        {
            var validateRestaurantAddress = await _addressValidationService.ValidateAddressAsync(dtoCreateRestaurant.City, dtoCreateRestaurant.Street, dtoCreateRestaurant.HouseNumber);
            var validateRestaurant = await _validationRestaurantService.ValidateCreateRestaurantAsync(dtoCreateRestaurant);       
            if (!validateRestaurantAddress.IsValid)
            {
                throw new InvalidOperationException(validateRestaurantAddress.Message);
            }
            if (!validateRestaurant.IsValid)
            {
                throw new InvalidOperationException(validateRestaurant.Message);
            }
           
            var restaurant = _mapper.Map<Restaurant>(dtoCreateRestaurant);
            await _restaurantRepository.AddRestaurant(restaurant);
            await _cacheService.RemoveByPrefixAsync(prefixs);
        }

        public async Task DeleteRestaurant(Guid Id)
        {
           
            await _restaurantRepository.DeleteRestaurant(Id);
            await _cacheService.RemoveByPrefixAsync(prefixs);
        }

        public async Task<PagedResult<DtoRestaurantInfo>> GetActiveRestaurants(PageParams pageParams)
        {
            var restaurants = await _restaurantRepository.GetActiveRestaurants(pageParams);
            string key = $"restaurants_active_page_{pageParams.Page}_size_{pageParams.PageSize}";
            if(restaurants == null)
            {
                throw new Exception("Активных ресторанов сейчас нету");
            }
            var dtoRestaurants = restaurants.Data.Select(x => new DtoRestaurantInfo
            {
                Name = x.Name,
                Description = x.Description,
                City = x.City,
                Street = x.Street,
                HouseNumber = x.HouseNumber,
                Apartment = x.Apartment,
                Phone = x.Phone,
                CuisineType = x.CuisineType,
                ImageUrl = x.ImageUrl,
                TimeClose = x.TimeClose,
                TimeOpen = x.TimeOpen,
                IsOpen = _statusService.IsActiveRestaurant(x.TimeOpen, x.TimeClose),
                IsActive = x.IsActive,
                Rating = x.Rating,
                DeliveryTime = x.DeliveryTime,
                DeliveryFee = x.DeliveryFee,
                CreatedAt = x.CreatedAt
            }).ToList();
            
            var CacheRestaurants = await _cacheService.CachingAsync(key, async () => dtoRestaurants, TimeSpan.FromMinutes(15));
            return new PagedResult<DtoRestaurantInfo>(CacheRestaurants, restaurants.Count);
        }

        public async Task<PagedResult<DtoRestaurantInfo>> GetRestaurantsByFilter(RestaurantFilter restaurantFilter, PageParams pageParams)
        {
            var restaurants = await _restaurantRepository.GetRestaurantsByFilter(restaurantFilter, pageParams);
            string key = $"restaurants_filter_city_{restaurantFilter.City}_cuisine_{restaurantFilter.CuisineType}_page_{pageParams.Page}_size_{pageParams.PageSize}";
            var dtoRestaurants = restaurants.Data.Select(x => new DtoRestaurantInfo
            {
                Name = x.Name,
                Description = x.Description,
                City = x.City,
                Street = x.Street,
                HouseNumber = x.HouseNumber,
                Apartment = x.Apartment,
                Phone = x.Phone,
                CuisineType = x.CuisineType,
                ImageUrl = x.ImageUrl,
                TimeClose = x.TimeClose,
                TimeOpen = x.TimeOpen,
                IsOpen = _statusService.IsActiveRestaurant(x.TimeOpen, x.TimeClose),
                IsActive = x.IsActive,
                Rating = x.Rating,
                DeliveryTime = x.DeliveryTime,
                DeliveryFee = x.DeliveryFee,
                CreatedAt = x.CreatedAt
            }).ToList();
             var CacheRestaurants = await _cacheService.CachingAsync(key, async () => dtoRestaurants, TimeSpan.FromMinutes(15));
             return new PagedResult<DtoRestaurantInfo>(CacheRestaurants, restaurants.Count);
        }

        public async Task<DtoRestaurantInfo> GetRestaurantById(Guid Id)
        {
            var restaurant = await _restaurantRepository.GetRestaurantById(Id);
            if(restaurant == null)
            {
                throw new InvalidOperationException("Ресторана с таким айди не существует");
            }
            return _mapper.Map<DtoRestaurantInfo>(restaurant);
        }

        public async Task<DtoRestaurantInfo> GetRestaurantByName(string RestaurantName)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByName(RestaurantName);
            if (restaurant == null)
            {
                throw new InvalidOperationException("Ресторан с таким названием не найден");
            }           
            return _mapper.Map<DtoRestaurantInfo>(restaurant);
        }

        public async Task<PagedResult<DtoRestaurantInfo>> GetRestaurantIsOpen(PageParams pageParams)
        {
            var restaurants = await _restaurantRepository.GetRestaurantIsOpenAsync(pageParams);
            string key = $"restaurants_open_page_{pageParams.Page}_size_{pageParams.PageSize}";
            var dtoRestaurant =  restaurants.Data.Select( x => new DtoRestaurantInfo
            {
                Name = x.Name,
                Description = x.Description,
                City = x.City,
                Street = x.Street,
                HouseNumber = x.HouseNumber,
                Apartment = x.Apartment,
                Phone = x.Phone,
                CuisineType = x.CuisineType,
                ImageUrl = x.ImageUrl,
                TimeClose = x.TimeClose,
                TimeOpen = x.TimeOpen,
                IsOpen = true,
                IsActive = x.IsActive,
                Rating = x.Rating,
                DeliveryTime = x.DeliveryTime,
                DeliveryFee = x.DeliveryFee,
                CreatedAt = x.CreatedAt
            }).ToList();
            var CacheRestaurants = await _cacheService.CachingAsync(key, async () => dtoRestaurant, TimeSpan.FromMinutes(15));
            return new PagedResult<DtoRestaurantInfo>(CacheRestaurants, restaurants.Count);
        }

        public async Task<PagedResult<DtoRestaurantInfo>> GetRestaurants(PageParams pageParams)
        {
            var restaurants = await _restaurantRepository.GetRestaurants(pageParams);
            string key = $"restaurants_all_page_{pageParams.Page}_size_{pageParams.PageSize}";

            var dtoRestaurant = restaurants.Data.Select( x => new DtoRestaurantInfo
            {
                Name = x.Name,
                Description = x.Description,
                City = x.City,
                Street = x.Street,
                HouseNumber = x.HouseNumber,
                Apartment = x.Apartment,
                Phone = x.Phone,
                CuisineType = x.CuisineType,
                ImageUrl = x.ImageUrl,
                TimeClose = x.TimeClose,
                TimeOpen = x.TimeOpen,
                IsOpen = _statusService.IsActiveRestaurant(x.TimeOpen, x.TimeClose),
                IsActive = x.IsActive,
                Rating = x.Rating,
                DeliveryTime = x.DeliveryTime,
                DeliveryFee = x.DeliveryFee,
                CreatedAt = x.CreatedAt
            }).ToList();
            
            var CacheRestaurants = await _cacheService.CachingAsync(key, async () => dtoRestaurant, TimeSpan.FromMinutes(15));
            return new PagedResult<DtoRestaurantInfo>(CacheRestaurants, restaurants.Count);
        }

        public async Task UpdateRatingRestaurant(Guid Id, decimal rating)
        {
            if (rating > 5 || rating <1)
            {
                throw new InvalidOperationException("Оценка должна быть от 1 до 5");
            }
            await _restaurantRepository.UpdateRatingRestaurant(Id, rating);
            await _cacheService.RemoveByPrefixAsync(prefixs);
        }

        public async Task<DtoRestaurantInfo> UpdateRestaurant(Guid Id, DtoUpdateRestaurant dtoUpdateRestaurant)
        {
            var validateRestaurant = await _validationRestaurantService.ValidateUpdateRestaurantAsync(Id, dtoUpdateRestaurant);
            if (!validateRestaurant.IsValid)
            {
                throw new InvalidOperationException(validateRestaurant.Message);
            }
            var restaurant = _mapper.Map<Restaurant>(dtoUpdateRestaurant);
            var updaterestaurant = await _restaurantRepository.UpdateRestaurant(Id, restaurant);
            await _cacheService.RemoveByPrefixAsync(prefixs);
            return _mapper.Map<DtoRestaurantInfo>(updaterestaurant);
        }

       
    }
}