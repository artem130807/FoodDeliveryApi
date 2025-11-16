using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Models
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
        public MenuCategory MenuCategory { get; set; }
        public Restaurant Restaurant { get; set; }
        public Guid CategoryId { get; set; }
        public Guid RestaurantId { get; set; }
    }
}