using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Models
{
    public class MenuCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
        public int DisplayOrder { get; set; }
    }
}