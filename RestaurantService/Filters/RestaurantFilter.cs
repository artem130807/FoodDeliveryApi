using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Filters
{
    public class RestaurantFilter
    {
        public string? CuisineType { get; set; }
        public string? City { get; set; }
        public decimal? Rating { get; set; }
        public int? DeliveryTime { get; set; }
        public decimal? DeliveryFee { get; set; }    
    }
}