using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Contracts;
using RestaurantService.Dto.DtoRestaurant;
using RestaurantService.Filters;
using RestaurantService.Models;
using RestaurantService.Page;

namespace RestaurantService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IAddressValidationService _addressValidationService;
        private readonly IValidationRestaurantService _validationRestaurantService;
        private readonly IRestaurantServices _restaurantServices;
        public RestaurantController(IRestaurantServices restaurantServices, IAddressValidationService addressValidationService, IValidationRestaurantService validationRestaurantService)
        {
            _restaurantServices = restaurantServices;
            _addressValidationService = addressValidationService;
            _validationRestaurantService = validationRestaurantService;
        }
        [HttpGet("get-restaurant")]
        public async Task<IActionResult> GetRestaurants([FromQuery] PageParams pageParams)
        {
            var restaurant = await _restaurantServices.GetRestaurants(pageParams);
            return Ok(restaurant);
        }
        [HttpGet("get-restaurantByName")]
        public async Task<IActionResult> GetRestaurantByName(string name)
        {
            
                var restaurant = await _restaurantServices.GetRestaurantByName(name);
                return Ok(restaurant);
           

        }
        [HttpGet("get-restaurantById{Id}")]
        public async Task<IActionResult> GetRestaurantById(Guid Id)
        {
            
                var restaurant = await _restaurantServices.GetRestaurantById(Id);
                return Ok(restaurant);
           
        }
        [HttpGet("get-restaurantsByFilter")]
        public async Task<IActionResult> GetRestaurantsByFilter([FromQuery] RestaurantFilter restaurantFilter, [FromQuery] PageParams pageParams)
        {
            var restaurants = await _restaurantServices.GetRestaurantsByFilter(restaurantFilter, pageParams);
            return Ok(restaurants);
        }
        [HttpGet("get-restaurantsByIsActive")]
        public async Task<IActionResult> GetRestaurantIsActive([FromQuery] PageParams pageParams)
        {
            var restaurant = await _restaurantServices.GetActiveRestaurants(pageParams);
            return Ok(restaurant);
        }

        [HttpGet("get-RestaurantsIsOpen")]
        public async Task<IActionResult> GetRestaurantIsOpen([FromQuery] PageParams pageParams)
        {
            var restaurants = await _restaurantServices.GetRestaurantIsOpen(pageParams);
            return Ok(restaurants);
        }
        [HttpDelete("delete-restaurant{Id}")]
        public async Task<IActionResult> DeleteRestaurantById(Guid Id)
        {
            await _restaurantServices.DeleteRestaurant(Id);
            return Ok("Успешно");
        }
        [HttpPost("create-restaurant")]
        public async Task<IActionResult> CreateRestaurant([FromBody] DtoCreateRestaurant dtoCreateRestaurant)
        {
           
                var createrestaurant = new DtoCreateRestaurant
                {
                    Name = dtoCreateRestaurant.Name,
                    Description = dtoCreateRestaurant.Description,
                    City = dtoCreateRestaurant.City,
                    Street = dtoCreateRestaurant.Street,
                    HouseNumber = dtoCreateRestaurant.HouseNumber,
                    Apartment = dtoCreateRestaurant.Apartment,
                    Phone = dtoCreateRestaurant.Phone,
                    CuisineType = dtoCreateRestaurant.CuisineType,
                    ImageUrl = dtoCreateRestaurant.ImageUrl,
                    TimeOpen = dtoCreateRestaurant.TimeOpen,
                    TimeClose = dtoCreateRestaurant.TimeClose,
                    IsActive = true,
                    Rating = 0,
                    DeliveryTime = dtoCreateRestaurant.DeliveryTime,
                    DeliveryFee = dtoCreateRestaurant.DeliveryFee,
                    CreatedAt = DateTime.UtcNow
                };
                await _restaurantServices.AddRestaurant(createrestaurant);
                return Ok("Успешно");
            
                    
        }
        [HttpPatch("update-restaurant/{Id}")]
        public async Task<IActionResult> UpdateRestaurant([FromBody] DtoUpdateRestaurant dtoUpdateRestaurant, Guid Id)
        {
            
                var updateRestaurant = new DtoUpdateRestaurant
                {
                    Name = dtoUpdateRestaurant.Name,
                    Description = dtoUpdateRestaurant.Description,
                    City = dtoUpdateRestaurant.City,
                    Street = dtoUpdateRestaurant.Street,
                    HouseNumber = dtoUpdateRestaurant.HouseNumber,
                    Apartment = dtoUpdateRestaurant.Apartment,
                    Phone = dtoUpdateRestaurant.Phone,
                    CuisineType = dtoUpdateRestaurant.CuisineType,
                    ImageUrl = dtoUpdateRestaurant.ImageUrl,
                    TimeOpen = dtoUpdateRestaurant.TimeOpen,
                    TimeClose = dtoUpdateRestaurant.TimeClose,
                    IsActive =  true,
                    Rating = dtoUpdateRestaurant.Rating,
                    DeliveryTime = dtoUpdateRestaurant.DeliveryTime,
                    DeliveryFee = dtoUpdateRestaurant.DeliveryFee,
                    CreatedAt = DateTime.UtcNow
                };
                var newRestaurant = await _restaurantServices.UpdateRestaurant(Id, updateRestaurant);
                return Ok(newRestaurant);
                     
        }
        [HttpPatch("update-restaurantRating/{Id}")]
        public async Task<IActionResult> UpdateRating(Guid Id, decimal Rating)
        {
            
                await _restaurantServices.UpdateRatingRestaurant(Id, Rating);
                return Ok("Успешно");
            
        }

    }
}