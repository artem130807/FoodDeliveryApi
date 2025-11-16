using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Dto.DtoRestaurant
{
    public class DtoCreateRestaurant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; } 
        public string? Apartment { get; set; }
        public string Phone { get; set; }
        public string CuisineType { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan TimeOpen { get; set; }
        public TimeSpan TimeClose { get; set; }
        public decimal Rating { get; set; } 
        public int DeliveryTime { get; set; }
        public decimal DeliveryFee { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}